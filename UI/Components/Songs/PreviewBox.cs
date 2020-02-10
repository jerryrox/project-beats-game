using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Audio;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

namespace PBGame.UI.Components.Songs
{
    public class PreviewBox : UguiTrigger, IPreviewBox {

        private ISprite mask;
        private ITexture thumb;
        private IProgressBar progressBar;
        private ILabel titleLabel;
        private ILabel artistLabel;
        private ISprite glow;

        private UIGradient thumbGradient;

        private IAnime hoverAni;
        private IAnime outAni;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }


        [InitWithDependency]
        private void Init(ISoundPooler soundPooler)
        {
            OnPointerEnter += () =>
            {
                soundPooler.Play("menuhit");

                outAni.Stop();
                hoverAni.PlayFromStart();
            };
            OnPointerExit += () =>
            {
                hoverAni.Stop();
                outAni.PlayFromStart();
            };
            OnPointerDown += () =>
            {
                soundPooler.Play("menuclick");
            };

            mask = CreateChild<UguiSprite>("mask", 0);
            {
                mask.Anchor = Anchors.Fill;
                mask.RawSize = Vector2.zero;
                mask.Color = HexColor.Create("2A313A");
                mask.SpriteName = "parallel-64";
                mask.ImageType = Image.Type.Sliced;

                mask.AddEffect(new MaskEffect());
                mask.AddEffect(new FlipEffect()).Component.horizontal = true;

                thumb = mask.CreateChild<UguiTexture>("thumb", 0);
                {
                    thumb.Anchor = Anchors.Fill;
                    thumb.RawSize = Vector2.zero;

                    thumbGradient = thumb.AddEffect(new GradientEffect()).Component;
                    thumbGradient.color1 = Color.black;
                    thumbGradient.offset = -0.5f;
                }
                progressBar = mask.CreateChild<UguiProgressBar>("progress", 1);
                {
                    progressBar.Anchor = Anchors.BottomStretch;
                    progressBar.Pivot = Pivots.Bottom;
                    progressBar.OffsetLeft = 22;
                    progressBar.OffsetRight = 0f;
                    progressBar.Height = 6f;
                    progressBar.Y = 0f;

                    progressBar.Background.Color = new Color(0f, 0f, 0f, 0.5f);
                    progressBar.Foreground.OffsetTop = 2f;
                }
                titleLabel = mask.CreateChild<Label>("title", 2);
                {
                    titleLabel.Anchor = Anchors.TopStretch;
                    titleLabel.Pivot = Pivots.Top;
                    titleLabel.OffsetLeft = 20f;
                    titleLabel.OffsetRight = 32f;
                    titleLabel.Y = -8f;
                    titleLabel.Height = 30f;
                    titleLabel.IsBold = true;
                    titleLabel.FontSize = 22;
                    titleLabel.Alignment = TextAnchor.UpperLeft;
                    titleLabel.WrapText = true;

                    var shadow = titleLabel.AddEffect(new ShadowEffect()).Component;
                    shadow.style = ShadowStyle.Outline;
                    shadow.effectDistance = Vector2.one;
                    shadow.effectColor = new Color(0f, 0f, 0f, 0.5f);
                }
                artistLabel = mask.CreateChild<Label>("artist", 3);
                {
                    artistLabel.Anchor = Anchors.BottomStretch;
                    artistLabel.Pivot = Pivots.Bottom;
                    artistLabel.OffsetLeft = 32f;
                    artistLabel.OffsetRight = 20f;
                    artistLabel.Y = 8f;
                    artistLabel.Height = 30f;
                    artistLabel.Alignment = TextAnchor.LowerLeft;
                    artistLabel.WrapText = true;

                    var shadow = artistLabel.AddEffect(new ShadowEffect()).Component;
                    shadow.style = ShadowStyle.Outline;
                    shadow.effectDistance = Vector2.one;
                    shadow.effectColor = new Color(0f, 0f, 0f, 0.5f);
                }
            }
            glow = CreateChild<UguiSprite>("glow", 1);
            {
                glow.Anchor = Anchors.Fill;
                glow.RawSize = new Vector2(28f, 30f);
                glow.Color = Color.black;
                glow.SpriteName = "glow-parallel-64";
                glow.ImageType = Image.Type.Sliced;

                glow.AddEffect(new FlipEffect()).Component.horizontal = true;
            }

            hoverAni = new Anime();
            hoverAni.AnimateColor(color => glow.Color = color)
                .AddTime(0f, () => glow.Color)
                .AddTime(0.25f, () => MapSelection.Background.Highlight)
                .Build();
            hoverAni.AnimateFloat(offset => thumbGradient.offset = offset)
                .AddTime(0f, () => thumbGradient.offset)
                .AddTime(0.25f, -1)
                .Build();

            outAni = new Anime();
            outAni.AnimateColor(color => glow.Color = color)
                .AddTime(0f, () => glow.Color)
                .AddTime(0.25f, Color.black)
                .Build();
            outAni.AnimateFloat(offset => thumbGradient.offset = offset)
                .AddTime(0f, () => thumbGradient.offset)
                .AddTime(0.25f, -0.5f)
                .Build();

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            BindEvents();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            UnbindEvents();
        }

        /// <summary>
        /// Binds events from external dependencies.
        /// </summary>
        private void BindEvents()
        {
            MapSelection.OnBackgroundLoaded += OnBackgroundChange;
            MapSelection.OnMapChange += OnMapChange;
            GameConfiguration.PreferUnicode.OnValueChanged += OnConfigChange;

            OnBackgroundChange(MapSelection.Background);
            OnMapChange(MapSelection.Map);
        }

        /// <summary>
        /// Unbinds events from external dependencies.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.OnBackgroundLoaded -= OnBackgroundChange;
            MapSelection.OnMapChange -= OnMapChange;
            GameConfiguration.PreferUnicode.OnValueChanged -= OnConfigChange;
        }

        /// <summary>
        /// Event called when the map background has been changed.
        /// </summary>
        private void OnBackgroundChange(IMapBackground background)
        {
            thumb.Active = background.Image != null;
            thumb.Texture = background.Image;
            thumb.FillTexture();
        }

        /// <summary>
        /// Event called when the selected map changes to the specifiedm map.
        /// </summary>
        private void OnMapChange(IMap map) => SetLabels(map, GameConfiguration.PreferUnicode.Value);

        /// <summary>
        /// Event called when the game configuration for prefer unicode has changed.
        /// </summary>
        private void OnConfigChange(bool preferUnicode, bool _) => SetLabels(MapSelection.Map, preferUnicode);

        /// <summary>
        /// Sets label values.
        /// </summary>
        private void SetLabels(IMap map, bool preferUnicode)
        {
            if (map == null)
            {
                titleLabel.Text = "";
                artistLabel.Text = "";
            }
            else
            {
                titleLabel.Text = map.Metadata.GetTitle(preferUnicode);
                artistLabel.Text = map.Metadata.GetArtist(preferUnicode);
            }
        }

        private void Update()
        {
            if(MusicController.Audio == null)
                progressBar.Value = 0f;
            else
                progressBar.Value = MusicController.CurrentTime / MusicController.Audio.Duration;
        }
    }
}