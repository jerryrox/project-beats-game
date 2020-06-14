using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Maps;
using PBGame.Audio;
using PBGame.Assets.Caching;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Allocation.Caching;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

using ShadowEffect = PBFramework.Graphics.Effects.CoffeeUI.ShadowEffect;

namespace PBGame.UI.Components.Songs
{
    public class SongListItem : BasicTrigger, IListItem {

        private const float AnimationSpeed = 4f;

        /// <summary>
        /// Number of seconds to wait before actually loading the background asset.
        /// </summary>
        private const float BackgroundLoadWait = 1f;

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
        private bool isAnimating = false;
        private bool isFocused = false;
        private bool isBackgroundWait = false;
        private float aniTime = 1f;
        private float bgWaitTime = BackgroundLoadWait;


        public int ItemIndex { get; set; }

        /// <summary>
        /// Returns the mapset currently being represented by this item.
        /// </summary>
        public IMapset Mapset { get; private set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IBackgroundCacher BackgroundCacher { get; set; }


        [InitWithDependency]
        private void Init()
        {
            IsClickToTrigger = true;

            OnTriggered += () =>
            {
                if(Active && Mapset != null)
                    MapSelection.SelectMapset(Mapset);
            };

            container = CreateChild<UguiObject>("container", 0);
            {
                container.Anchor = AnchorType.CenterStretch;
                container.SetOffsetVertical(5f);
                container.Width = UnfocusedWidth;

                highlight = container.CreateChild<UguiSprite>("highlight", 0);
                {
                    highlight.Size = new Vector2(1920f, 144f);
                    highlight.SpriteName = "glow-128";
                    highlight.Alpha = UnfocusedHighlightAlpha;

                    if(highlight is IRaycastable raycastable)
                        raycastable.IsRaycastTarget = false;

                    highlight.AddEffect(new AdditiveShaderEffect());
                }
                glow = container.CreateChild<UguiSprite>("glow", 1);
                {
                    glow.Anchor = AnchorType.Fill;
                    glow.RawSize = new Vector2(30f, 30f);
                    glow.SpriteName = "glow-circle-32";
                    glow.ImageType = Image.Type.Sliced;
                    glow.Color = UnfocusedGlowColor;
                }
                thumbContainer = container.CreateChild<UguiSprite>("thumb", 2);
                {
                    thumbContainer.Anchor = AnchorType.Fill;
                    thumbContainer.RawSize = Vector2.zero;
                    thumbContainer.SpriteName = "circle-32";
                    thumbContainer.ImageType = Image.Type.Sliced;
                    thumbContainer.Color = Color.black;

                    thumbContainer.AddEffect(new MaskEffect());

                    thumbImage = thumbContainer.CreateChild<UguiTexture>("image");
                    {
                        thumbImage.Anchor = AnchorType.Fill;
                        thumbImage.RawSize = Vector2.zero;
                        thumbImage.Color = UnfocusedThumbColor;
                    }
                }
                titleLabel = container.CreateChild<Label>("title", 3);
                {
                    titleLabel.Anchor = AnchorType.TopStretch;
                    titleLabel.Pivot = PivotType.Top;
                    titleLabel.Y = -8f;
                    titleLabel.Height = 32f;
                    titleLabel.SetOffsetHorizontal(20f);
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
                    artistLabel.Anchor = AnchorType.BottomStretch;
                    artistLabel.Pivot = PivotType.Bottom;
                    artistLabel.Y = 8f;
                    artistLabel.Height = 24f;
                    artistLabel.SetOffsetHorizontal(20f);
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
                    creatorLabel.Anchor = AnchorType.BottomStretch;
                    creatorLabel.Pivot = PivotType.Bottom;
                    creatorLabel.Y = 8f;
                    creatorLabel.Height = 24f;
                    creatorLabel.SetOffsetHorizontal(20f);
                    creatorLabel.WrapText = true;
                    creatorLabel.Alignment = TextAnchor.MiddleRight;
                    creatorLabel.FontSize = 18;

                    creatorShadow = creatorLabel.AddEffect(new ShadowEffect()).Component;
                    creatorShadow.style = ShadowStyle.Shadow;
                    creatorShadow.effectColor = Color.black;
                    creatorShadow.enabled = false;
                }
            }

            backgroundAgent = new CacherAgent<IMap, IMapBackground>(BackgroundCacher)
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

            SetMapset(null);
            UnbindEvents();
        }

        /// <summary>
        /// Sets the mapset which the item should represent.
        /// </summary>
        public void SetMapset(IMapset mapset)
        {
            this.Mapset = mapset;

            // Set info label texts.
            SetLabelText();

            // Start loading mapset backgrond.
            LoadBackground();

            // Set mapset focus.
            SetFocus(MapSelection.Mapset != null && this.Mapset == MapSelection.Mapset);
        }

        /// <summary>
        /// Binds events to external dependencies.
        /// </summary>
        private void BindEvents()
        {
            MapSelection.OnMapsetChange += OnMapsetChanged;
            GameConfiguration.PreferUnicode.OnValueChanged += OnPreferUnicode;

            OnMapsetChanged(MapSelection.Mapset);
            OnPreferUnicode(GameConfiguration.PreferUnicode.Value, false);
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

            // Background asset should be loaded after a certain delay.
            if (Mapset != null)
            {
                isBackgroundWait = true;
                if(BackgroundCacher.IsCached(Mapset.Maps[0]))
                    bgWaitTime = 0f;
                else
                    bgWaitTime = BackgroundLoadWait;
            }
        }

        /// <summary>
        /// Sets the item ready for animation based on state change.
        /// </summary>
        private void StartAnimation()
        {
            aniTime = 0f;
            isAnimating = true;
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
            if(Mapset == null)
                return;
            if (backgroundAgent.CurrentKey != Mapset.Maps[0])
            {
                LoadBackground();
                return;
            }

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

        protected override void Update()
        {
            base.Update();
            
            // Load background.
            if (isBackgroundWait)
            {
                bgWaitTime -= Time.deltaTime;
                if (bgWaitTime <= 0f)
                {
                    bgWaitTime = BackgroundLoadWait;
                    isBackgroundWait = false;

                    if(Mapset != null)
                        backgroundAgent.Request(Mapset.Maps[0]);
                }
            }

            if(!isAnimating) return;

            // Advance
            aniTime += Time.deltaTime * AnimationSpeed;

            // Stop animation.
            if (aniTime >= 1f)
            {
                aniTime = 1f;
                isAnimating = false;
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

            // Keep position at 0.
            // TODO: This seems more of a hack. This should be handled in a better way in future.
            X = 0f;
        }
    }
}