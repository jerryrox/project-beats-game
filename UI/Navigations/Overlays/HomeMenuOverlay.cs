using PBGame.UI.Models;
using PBGame.UI.Components.Common;
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
    public class HomeMenuOverlay : BaseOverlay<HomeMenuModel> {

        private BlurDisplay blur;
        private ISprite gradientSprite;
        private MenuButton quitButton;
        private MenuButton backButton;
        private MenuButton playButton;
        private MenuButton downloadButton;

        private GradientEffect gradientEffect;


        protected override int ViewDepth => ViewDepths.HomeMenuOverlay;


        [InitWithDependency]
        private void Init(IMapSelection mapSelection)
        {
            blur = CreateChild<BlurDisplay>("focus-blur", 0);
            {
                blur.Anchor = AnchorType.Fill;
                blur.Offset = Offset.Zero;
            }
            gradientSprite = CreateChild<UguiSprite>("gradient", 1);
            {
                gradientSprite.Anchor = AnchorType.Fill;
                gradientSprite.RawSize = Vector2.zero;

                gradientEffect = gradientSprite.AddEffect(new GradientEffect());
                gradientEffect.Component.direction = UIGradient.Direction.Vertical;
            }
            quitButton = CreateChild<MenuButton>("quit-button", 2);
            {
                quitButton.Anchor = AnchorType.Bottom;
                quitButton.Y = 100;
                quitButton.Size = new Vector2(160f, 160f);
                quitButton.LabelText = "Quit";
                quitButton.IconName = "icon-power";

                quitButton.OnTriggered += OnQuitButton;
            }
            backButton = CreateChild<MenuButton>("back-button", 2);
            {
                backButton.X = -160f;
                backButton.Size = new Vector2(160f, 160f);
                backButton.LabelText = "Back";
                backButton.IconName = "icon-arrow-left";

                backButton.OnTriggered += OnBackButton;
            }
            playButton = CreateChild<MenuButton>("play-button", 2);
            {
                playButton.Size = new Vector2(160f, 160f);
                playButton.LabelText = "Play";
                playButton.IconName = "icon-play";

                playButton.OnTriggered += OnPlayButton;
            }
            downloadButton = CreateChild<MenuButton>("download-button", 2);
            {
                downloadButton.X = 160f;
                downloadButton.Size = new Vector2(160f, 160f);
                downloadButton.LabelText = "Download";
                downloadButton.IconName = "icon-download";

                downloadButton.OnTriggered += OnDownloadButton;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.Background.BindAndTrigger(OnBackgroundChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Background.OnNewValue -= OnBackgroundChange;
        }

        /// <summary>
        /// Event called on quit button press.
        /// </summary>
        private void OnQuitButton() => model.QuitGame();

        /// <summary>
        /// Event called on back button press.
        /// </summary>
        private void OnBackButton() => model.HideMenu();

        /// <summary>
        /// Event called on play button press.
        /// </summary>
        private void OnPlayButton() => model.PlayGame();

        /// <summary>
        /// Event called on download button press.
        /// </summary>
        private void OnDownloadButton() => model.DownloadMaps();

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