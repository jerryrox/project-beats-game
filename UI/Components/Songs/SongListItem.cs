using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Assets.Caching;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Allocation.Caching;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

namespace PBGame.UI.Components.Songs
{
    public class SongListItem : UguiTrigger, ISongListItem {

        private const float AnimationSpeed = 4f;
        private const float FocusedWidth = 1100f;
        private const float UnfocusedWidth = 1000f;

        private const float UnfocusedHighlightAlpha = 0f;
        private const float FocusedHighlightAlpha = 1f;
        private static readonly Color UnfocusedGlowColor = new Color(0f, 0f, 0f, 0.4f);
        private static readonly Color DefaultGlowColor = Color.white;
        private const float FocusedGlowAlpha = 0.5f;
        private static readonly Color UnfocusedThumbColor = Color.gray;
        private static readonly Color FocusedThumbColor = Color.white;

        private IGraphicObject container;
        private ISprite highlight;
        private ISprite glow;
        private ISprite thumbContainer;
        private ITexture thumbImage;
        private ILabel titleLabel;
        private ILabel artistLabel;
        private ILabel creatorLabel;

        private UIShadow titleShadow;
        private UIShadow artistShadow;
        private UIShadow creatorShadow;

        private CacherAgent<IMap, IMapBackground> backgroundAgent;
        private bool isFocused = false;
        private float aniTime = 1f;


        public int ItemIndex { get; set; }

        public IMapset Mapset { get; private set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init(IBackgroundCacher backgroundCacher)
        {
            container = CreateChild<UguiObject>("container", 0);
            {
                container.Anchor = Anchors.CenterStretch;
                container.OffsetTop = 5f;
                container.OffsetBottom = 5f;
                container.Width = UnfocusedWidth;

                highlight = container.CreateChild<UguiSprite>("highlight", 0);
                {
                    highlight.Size = new Vector2(1920f, 144f);
                    highlight.SpriteName = "glow-128";
                    highlight.Alpha = UnfocusedHighlightAlpha;

                    highlight.AddEffect(new AdditiveShaderEffect());
                }
                glow = container.CreateChild<UguiSprite>("glow", 1);
                {
                    glow.Anchor = Anchors.Fill;
                    glow.RawSize = new Vector2(30f, 30f);
                    glow.SpriteName = "glow-circle-32";
                    glow.ImageType = Image.Type.Sliced;
                    glow.Color = UnfocusedGlowColor;
                }
                thumbContainer = container.CreateChild<UguiSprite>("thumb", 2);
                {
                    thumbContainer.Anchor = Anchors.Fill;
                    thumbContainer.RawSize = Vector2.zero;
                    thumbContainer.SpriteName = "circle-32";
                    thumbContainer.ImageType = Image.Type.Sliced;
                    thumbContainer.Color = Color.black;

                    thumbContainer.AddEffect(new MaskEffect());

                    thumbImage = thumbContainer.CreateChild<UguiTexture>("image");
                    {
                        thumbImage.Anchor = Anchors.Fill;
                        thumbImage.RawSize = Vector2.zero;
                        thumbImage.Color = UnfocusedThumbColor;
                    }
                }
                titleLabel = container.CreateChild<Label>("title", 3);
                {
                    titleLabel.Anchor = Anchors.TopStretch;
                    titleLabel.Pivot = Pivots.Top;
                    titleLabel.Y = -8f;
                    titleLabel.Height = 32f;
                    titleLabel.OffsetLeft = 20f;
                    titleLabel.OffsetRight = 20f;
                    titleLabel.IsItalic = true;
                    titleLabel.IsBold = true;
                    titleLabel.WrapText = true;
                    titleLabel.Alignment = TextAnchor.MiddleLeft;
                    titleLabel.FontSize = 22;

                    titleShadow = titleLabel.AddEffect(new ShadowEffect()).Component;
                    titleShadow.style = ShadowStyle.Shadow;
                    titleShadow.effectColor = Color.black;
                    titleShadow.enabled = false;
                }
                artistLabel = container.CreateChild<Label>("artist", 4);
                {
                    artistLabel.Anchor = Anchors.BottomStretch;
                    artistLabel.Pivot = Pivots.Bottom;
                    artistLabel.Y = 8f;
                    artistLabel.Height = 24f;
                    artistLabel.OffsetLeft = 20f;
                    artistLabel.OffsetRight = 20f;
                    artistLabel.WrapText = true;
                    artistLabel.Alignment = TextAnchor.MiddleLeft;
                    artistLabel.FontSize = 18;

                    artistShadow = artistLabel.AddEffect(new ShadowEffect()).Component;
                    artistShadow.style = ShadowStyle.Shadow;
                    artistShadow.effectColor = Color.black;
                    artistShadow.enabled = false;
                }
                creatorLabel = container.CreateChild<Label>("creator", 5);
                {
                    creatorLabel.Anchor = Anchors.BottomStretch;
                    creatorLabel.Pivot = Pivots.Bottom;
                    creatorLabel.Y = 8f;
                    creatorLabel.Height = 24f;
                    creatorLabel.OffsetLeft = 20f;
                    creatorLabel.OffsetRight = 20f;
                    creatorLabel.WrapText = true;
                    creatorLabel.Alignment = TextAnchor.MiddleRight;
                    creatorLabel.FontSize = 18;

                    creatorShadow = creatorLabel.AddEffect(new ShadowEffect()).Component;
                    creatorShadow.style = ShadowStyle.Shadow;
                    creatorShadow.effectColor = Color.black;
                    creatorShadow.enabled = false;
                }
            }

            backgroundAgent = new CacherAgent<IMap, IMapBackground>(backgroundCacher)
            {
                UseDelayedRemove = true,
                RemoveDelay = 2f
            };
            backgroundAgent.OnFinished += OnBackgroundLoaded;

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

        public void SetMapset(IMapset mapset)
        {
            this.Mapset = mapset;

            // Set info label texts.
            SetLabelText();

            // Start loading mapset backgrond.
            LoadBackground();

            // Set mapset focus.
            SetFocus(mapset != null && MapSelection.Mapset == mapset);
        }

        /// <summary>
        /// Binds events to external dependencies.
        /// </summary>
        private void BindEvents()
        {
            MapSelection.OnMapsetChange += OnMapsetChanged;
            GameConfiguration.PreferUnicode.OnValueChanged += OnPreferUnicode;

            OnMapsetChanged(MapSelection.Mapset);
        }

        /// <summary>
        /// Unbinds events from external dependencies.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.OnMapsetChange -= OnMapsetChanged;
            GameConfiguration.PreferUnicode.OnValueChanged -= OnPreferUnicode;
        }

        /// <summary>
        /// Sets the focus flag of the item.
        /// </summary>
        private void SetFocus(bool isFocused)
        {
            this.isFocused = isFocused;
            StartAnimation();
        }

        /// <summary>
        /// Loads background asset from current mapset.
        /// </summary>
        private void LoadBackground()
        {
            backgroundAgent.Remove();

            SetHighlightColor(Color.white);
            thumbImage.Texture = null;
            thumbImage.Alpha = 0f;

            if (Mapset != null)
                backgroundAgent.Request(Mapset.Maps[0]);
        }

        /// <summary>
        /// Sets the item ready for animation based on state change.
        /// </summary>
        private void StartAnimation()
        {
            aniTime = 0f;
            enabled = true;
            Update();
        }

        /// <summary>
        /// Assigns highlight sprite's color without affecting its alpha.
        /// </summary>
        private void SetHighlightColor(Color color)
        {
            color.a = highlight.Alpha;
            highlight.Color = color;
        }

        /// <summary>
        /// Sets text label values.
        /// </summary>
        private void SetLabelText()
        {
            if (Mapset != null)
            {
                var useUnicode = GameConfiguration.PreferUnicode.Value;
                var metadata = Mapset.Metadata;

                titleLabel.Text = metadata.GetTitle(useUnicode);
                artistLabel.Text = metadata.GetArtist(useUnicode);
                creatorLabel.Text = metadata.Creator;
            }
            else
            {
                titleLabel.Text = "";
                artistLabel.Text = "";
                creatorLabel.Text = "";
            }
        }

        /// <summary>
        /// Event called on mapset selection change.
        /// </summary>
        private void OnMapsetChanged(IMapset mapset)
        {
            SetFocus(mapset != null && this.Mapset == mapset);
        }

        /// <summary>
        /// Event called on unicode preference option change.
        /// </summary>
        private void OnPreferUnicode(bool useUnicode, bool _) => SetLabelText();

        /// <summary>
        /// Event called from cacher agent when the mapset background has been loaded.
        /// </summary>
        private void OnBackgroundLoaded(IMapBackground background)
        {
            // Set highlight color without affecting current alpha.
            SetHighlightColor(background.Highlight);

            // Assign thumbnail image.
            thumbImage.Texture = background.Image;
            if (background.Image != null)
            {
                thumbImage.FillTexture();
                StartAnimation();
            }
        }

        private void Update()
        {
            // Advance
            aniTime += Time.deltaTime * AnimationSpeed;

            // Stop animation.
            if (aniTime >= 1f)
            {
                aniTime = 1f;
                enabled = false;
            }

            // Determine target transition values.
            var background = backgroundAgent.Value;

            float highlightAlpha, containerWidth;
            Color glowColor, thumbColor;
            if (!isFocused)
            {
                highlightAlpha = UnfocusedHighlightAlpha;
                containerWidth = UnfocusedWidth;
                glowColor = UnfocusedGlowColor;
                thumbColor = UnfocusedThumbColor;
            }
            else
            {
                highlightAlpha = FocusedHighlightAlpha;
                containerWidth = FocusedWidth;
                glowColor = background != null ? background.Highlight : DefaultGlowColor;
                glowColor.a = FocusedGlowAlpha;
                thumbColor = FocusedThumbColor;
            }
            thumbColor.a = thumbImage.Texture == null ? 0f : 1f;

            // Toggle shadow
            titleShadow.enabled = isFocused;
            artistShadow.enabled = isFocused;
            creatorShadow.enabled = isFocused;

            // Animate components.
            highlight.Alpha = Mathf.Lerp(highlight.Alpha, highlightAlpha, aniTime);
            container.Width = Mathf.Lerp(container.Width, containerWidth, aniTime);
            glow.Color = Color.Lerp(glow.Color, glowColor, aniTime);
            thumbImage.Color = Color.Lerp(thumbImage.Color, thumbColor, aniTime);
        }
    }
}