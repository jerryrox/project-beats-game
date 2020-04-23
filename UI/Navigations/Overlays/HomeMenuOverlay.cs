using System;
using PBGame.UI.Components.HomeMenu;
using PBGame.UI.Navigations.Screens;
using PBGame.Maps;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Dependencies;
using UnityEngine;
using Coffee.UIExtensions;

namespace PBGame.UI.Navigations.Overlays
{
    public class HomeMenuOverlay : BaseOverlay, IHomeMenuOverlay {

        private GradientEffect gradientEffect;


        public ISprite BlurSprite { get; private set; }

        public ISprite GradientSprite { get; private set; }

        public MenuButton QuitButton { get; private set; }

        public MenuButton BackButton { get; private set; }

        public MenuButton PlayButton { get; private set; }

        public MenuButton DownloadButton { get; private set; }

        protected override int OverlayDepth => ViewDepths.HomeMenuOverlay;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IGame Game { get; set; }


        [InitWithDependency]
        private void Init(IMapSelection mapSelection)
        {
            BlurSprite = CreateChild<UguiSprite>("focus-blur", 0);
            {
                BlurSprite.Anchor = Anchors.Fill;
                BlurSprite.RawSize = Vector2.zero;
                BlurSprite.SpriteName = "null";

                BlurSprite.AddEffect(new BlurShaderEffect());
            }
            GradientSprite = CreateChild<UguiSprite>("gradient", 1);
            {
                GradientSprite.Anchor = Anchors.Fill;
                GradientSprite.RawSize = Vector2.zero;

                gradientEffect = GradientSprite.AddEffect(new GradientEffect());
                gradientEffect.Component.direction = UIGradient.Direction.Vertical;
            }
            QuitButton = CreateChild<MenuButton>("quit-button", 2);
            {
                QuitButton.Anchor = Anchors.Bottom;
                QuitButton.Y = 100;
                QuitButton.Size = new Vector2(160f, 160f);
                QuitButton.LabelText = "Quit";
                QuitButton.IconName = "icon-power";

                QuitButton.OnTriggered += OnQuitButton;
            }
            BackButton = CreateChild<MenuButton>("back-button", 2);
            {
                BackButton.X = -160f;
                BackButton.Size = new Vector2(160f, 160f);
                BackButton.LabelText = "Back";
                BackButton.IconName = "icon-arrow-left";

                BackButton.OnTriggered += OnBackButton;
            }
            PlayButton = CreateChild<MenuButton>("play-button", 2);
            {
                PlayButton.Size = new Vector2(160f, 160f);
                PlayButton.LabelText = "Play";
                PlayButton.IconName = "icon-play";

                PlayButton.OnTriggered += OnPlayButton;
            }
            DownloadButton = CreateChild<MenuButton>("download-button", 2);
            {
                DownloadButton.X = 160f;
                DownloadButton.Size = new Vector2(160f, 160f);
                DownloadButton.LabelText = "Download";
                DownloadButton.IconName = "icon-download";

                DownloadButton.OnTriggered += OnDownloadButton;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            MapSelection.OnBackgroundLoaded += OnBackgroundChange;
            OnBackgroundChange(MapSelection.Background);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MapSelection.OnBackgroundLoaded -= OnBackgroundChange;
        }

        /// <summary>
        /// Event called on quit button press.
        /// </summary>
        private void OnQuitButton()
        {
            Game.GracefulQuit();
        }

        /// <summary>
        /// Event called on back button press.
        /// </summary>
        private void OnBackButton()
        {
            OverlayNavigator.Hide(this);
        }

        /// <summary>
        /// Event called on play button press.
        /// </summary>
        private void OnPlayButton()
        {
            ScreenNavigator.Show<SongsScreen>();
            OverlayNavigator.Hide(this);
        }

        /// <summary>
        /// Event called on download button press.
        /// </summary>
        private void OnDownloadButton()
        {
            // TODO: Show download screen.
            OverlayNavigator.Hide(this);
        }

        /// <summary>
        /// Event called when the map background has changed.
        /// </summary>
        private void OnBackgroundChange(IMapBackground background)
        {
            var highlightColor = background.Highlight;
            highlightColor.a = 0f;
            gradientEffect.Component.color1 = highlightColor;
            gradientEffect.Component.color2 = Color.black;
        }
    }
}