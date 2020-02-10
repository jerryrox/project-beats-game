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

        public event Action<bool> OnViewHide;

        private GradientEffect gradientEffect;


        public ISprite BlurSprite { get; private set; }

        public ISprite GradientSprite { get; private set; }

        public IMenuButton QuitButton { get; private set; }

        public IMenuButton BackButton { get; private set; }

        public IMenuButton PlayButton { get; private set; }

        public IMenuButton DownloadButton { get; private set; }

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
            QuitButton = CreateChild<QuitMenuButton>("quit-button", 2);
            {
                QuitButton.Anchor = Anchors.Bottom;
                QuitButton.Y = 100;
                QuitButton.Size = new Vector2(160f, 160f);
                QuitButton.OnPointerClick += OnQuitButton;
            }
            BackButton = CreateChild<BackMenuButton>("back-button", 2);
            {
                BackButton.X = -160f;
                BackButton.Size = new Vector2(160f, 160f);
                BackButton.OnPointerClick += OnBackButton;
            }
            PlayButton = CreateChild<PlayMenuButton>("play-button", 2);
            {
                PlayButton.Size = new Vector2(160f, 160f);
                PlayButton.OnPointerClick += OnPlayButton;
            }
            DownloadButton = CreateChild<DownloadMenuButton>("download-button", 2);
            {
                DownloadButton.X = 160f;
                DownloadButton.Size = new Vector2(160f, 160f);
                DownloadButton.OnPointerClick += OnDownloadButton;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            MapSelection.OnBackgroundLoaded += OnBackgroundChange;

            OnBackgroundChange(MapSelection.Background);
        }

        protected override void OnDisable()
        {
            MapSelection.OnBackgroundLoaded -= OnBackgroundChange;
        }

        /// <summary>
        /// Event called on quit button press.
        /// </summary>
        private void OnQuitButton()
        {
            // Confirm quit
            var dialog = OverlayNavigator.Show<DialogOverlay>();
            dialog.SetMessage("Are you sure you want to quit Project: Beats?");
            dialog.AddConfirmCancel(Game.GracefulQuit, null);
        }

        /// <summary>
        /// Event called on back button press.
        /// </summary>
        private void OnBackButton()
        {
            HideView(false);
        }

        /// <summary>
        /// Event called on play button press.
        /// </summary>
        private void OnPlayButton()
        {
            HideView(true);
            ScreenNavigator.Show<SongsScreen>();
        }

        /// <summary>
        /// Event called on download button press.
        /// </summary>
        private void OnDownloadButton()
        {
            HideView(true);
            // TODO: Show download screen.
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

        /// <summary>
        /// Hides this view.
        /// </summary>
        private void HideView(bool isTransitioning)
        {
            OnViewHide?.Invoke(isTransitioning);
            OverlayNavigator.Hide(this);
        }
    }
}