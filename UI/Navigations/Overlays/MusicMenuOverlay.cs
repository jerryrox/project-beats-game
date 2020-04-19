using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components;
using PBGame.UI.Components.MenuBar;
using PBGame.UI.Components.MusicMenu;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Graphics;
using PBGame.Animations;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Overlays
{
    public class MusicMenuOverlay : BaseSubMenuOverlay, IMusicMenuOverlay {

        private new ISprite mask;
        private ISprite glow;
        private IMapImageDisplay imageDisplay;
        private ISprite gradient;
        private ILabel title;
        private ILabel artist;
        private ControlButton randomButton;
        private ControlButton prevButton;
        private ControlButton playButton;
        private ControlButton nextButton;
        private TimeBar timeBar;


        public MusicButton MusicButton { get; set; }

        protected override int OverlayDepth => ViewDepths.MusicMenuOverlay;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            container.Anchor = Anchors.TopRight;
            container.Pivot = Pivots.TopRight;
            container.X = -16f;
            container.Y = -16f;
            container.Width = 400f;
            container.Height = 140f;

            mask = container.CreateChild<UguiSprite>("mask", 0);
            {
                mask.Anchor = Anchors.Fill;
                mask.RawSize = Vector2.zero;
                mask.Position = Vector2.zero;
                mask.SpriteName = "box";
                mask.Color = Color.black;

                imageDisplay = mask.CreateChild<MapImageDisplay>("imageDisplay", 0);
                {
                    imageDisplay.Anchor = Anchors.Fill;
                    imageDisplay.RawSize = Vector2.zero;
                }
                gradient = mask.CreateChild<UguiSprite>("gradient", 1);
                {
                    gradient.Anchor = Anchors.Fill;
                    gradient.OffsetTop = -22f;
                    gradient.OffsetLeft = 0f;
                    gradient.OffsetRight = 0f;
                    gradient.OffsetBottom = 0f;
                    gradient.SpriteName = "gradation-bottom";
                    gradient.Color = new Color(0f, 0f, 0f, 0.9f);
                }
                title = mask.CreateChild<Label>("title", 2);
                {
                    title.Anchor = Anchors.BottomStretch;
                    title.OffsetLeft = title.OffsetRight = 16f;
                    title.Y = 92f;
                    title.Height = 30f;
                    title.IsBold = true;
                    title.WrapText = false;
                    title.FontSize = 18;
                }
                artist = mask.CreateChild<Label>("artist", 3);
                {
                    artist.Anchor = Anchors.BottomStretch;
                    artist.OffsetLeft = artist.OffsetRight = 16f;
                    artist.Y = 70f;
                    artist.Height = 30f;
                    artist.WrapText = false;
                    artist.FontSize = 16;
                }
                randomButton = mask.CreateChild<ControlButton>("random", 4);
                {
                    randomButton.Anchor = Anchors.BottomLeft;
                    randomButton.X = 36f;
                    randomButton.Y = 36f;
                    randomButton.Size = new Vector2(48f, 48f);
                    randomButton.IconName = "icon-random";
                    randomButton.IconSize = 24f;

                    randomButton.OnTriggered += () =>
                    {
                        MusicButton.SetRandomMusic();
                    };
                }
                prevButton = mask.CreateChild<ControlButton>("prev", 5);
                {
                    prevButton.Anchor = Anchors.Bottom;
                    prevButton.X = -56f;
                    prevButton.Y = 36f;
                    prevButton.Size = new Vector2(48f, 48f);
                    prevButton.IconName = "icon-backward";
                    prevButton.IconSize = 24f;

                    prevButton.OnTriggered += () =>
                    {
                        MusicButton.SetPrevMusic();
                    };
                }
                playButton = mask.CreateChild<ControlButton>("play", 6);
                {
                    playButton.Anchor = Anchors.Bottom;
                    playButton.Y = 36f;
                    playButton.Size = new Vector2(48f, 48f);
                    playButton.IconName = "icon-play";
                    playButton.IconSize = 32f;

                    playButton.OnTriggered += () =>
                    {
                        if(MusicController.IsPlaying)
                            MusicController.Pause();
                        else if(MusicController.IsPaused)
                            MusicController.Play();
                    };
                }
                nextButton = mask.CreateChild<ControlButton>("next", 7);
                {
                    nextButton.Anchor = Anchors.Bottom;
                    nextButton.X = 56f;
                    nextButton.Y = 36f;
                    nextButton.Size = new Vector2(48f, 48f);
                    nextButton.IconName = "icon-forward";
                    nextButton.IconSize = 24f;

                    nextButton.OnTriggered += () =>
                    {
                        MusicButton.SetNextMusic();
                    };
                }
                timeBar = mask.CreateChild<TimeBar>("timebar", 8);
                {
                    timeBar.Anchor = Anchors.BottomStretch;
                    timeBar.Pivot = Pivots.Bottom;
                    timeBar.OffsetLeft = timeBar.OffsetRight = 0f;
                    timeBar.Y = 0f;
                    timeBar.Height = 8f;
                }
            }
            glow = container.CreateChild<UguiSprite>("glow", -1);
            {
                glow.Anchor = Anchors.Fill;
                glow.RawSize = new Vector2(30f, 30f);
                glow.Position = Vector2.zero;
                glow.SpriteName = "square-32-glow";
                glow.ImageType = Image.Type.Sliced;
                glow.Color = Color.black;
            }

            hoverAni = new Anime();
            hoverAni.AnimateColor(color => glow.Color = color)
                .AddTime(0f, () => glow.Color)
                .AddTime(0.25f, Color.gray)
                .Build();

            outAni = new Anime();
            outAni.AnimateColor(color => glow.Color = color)
                .AddTime(0f, () => glow.Color)
                .AddTime(0.25f, Color.black)
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

        protected override IAnime CreateShowAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>().GetSubMenuOverlayPopupShow(this, () => container);
        }

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>().GetSubMenuOverlayPopupHide(this, () => container);
        }

        /// <summary>
        /// Binds to events from external dependencies.
        /// </summary>
        private void BindEvents()
        {
            MusicController.OnPlay += OnMusicPlay;
            MusicController.OnPause += OnMusicPause;
            MusicController.OnUnpause += OnMusicUnpause;

            MapSelection.OnMapChange += OnMapChange;
            MapSelection.OnBackgroundLoaded += OnBackgroundChange;

            GameConfiguration.PreferUnicode.OnValueChanged += OnPreferUnicode;

            SetPlayButtonIcon();
            SetLabelText();
            OnBackgroundChange(MapSelection.Background);
        }

        /// <summary>
        /// Unbinds from events from external dependencies.
        /// </summary>
        private void UnbindEvents()
        {
            MusicController.OnPlay -= OnMusicPlay;
            MusicController.OnPause -= OnMusicPause;
            MusicController.OnUnpause -= OnMusicUnpause;

            MapSelection.OnMapChange -= OnMapChange;
            MapSelection.OnBackgroundLoaded -= OnBackgroundChange;

            GameConfiguration.PreferUnicode.OnValueChanged -= OnPreferUnicode;
        }

        /// <summary>
        /// Refreshes the play button icon.
        /// </summary>
        private void SetPlayButtonIcon()
        {
            playButton.IconName = MusicController.IsPlaying ? "icon-pause" : "icon-play";
        }

        /// <summary>
        /// Refreshes the song info label texts.
        /// </summary>
        private void SetLabelText()
        {
            var map = MapSelection.Map;
            if (map != null)
            {
                var preferUnicode = GameConfiguration.PreferUnicode.Value;

                title.Text = map.Metadata.GetTitle(preferUnicode);
                artist.Text = map.Metadata.GetArtist(preferUnicode);
            }
            else
            {
                title.Text = "";
                artist.Text = "";
            }
        }

        /// <summary>
        /// Event called when the map background has changed.
        /// </summary>
        private void OnBackgroundChange(IMapBackground background)
        {
            imageDisplay.SetBackground(background);
        }

        /// <summary>
        /// Event called when the selected map has changed.
        /// </summary>
        private void OnMapChange(IPlayableMap map) => SetLabelText();

        /// <summary>
        /// Event called when the unicode preference option has changed.
        /// </summary>
        private void OnPreferUnicode(bool preferUnicode, bool _) => SetLabelText();

        /// <summary>
        /// Event called from music controller when music is playing.
        /// </summary>
        private void OnMusicPlay(float time) => SetPlayButtonIcon();

        /// <summary>
        /// Event called from music controller when music is paused.
        /// </summary>
        private void OnMusicPause() => SetPlayButtonIcon();

        /// <summary>
        /// Event called from music controller when music is unpaused.
        /// </summary>
        private void OnMusicUnpause(float time) => SetPlayButtonIcon();
    }
}