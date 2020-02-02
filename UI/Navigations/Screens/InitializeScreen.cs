using PBGame.UI.Components.Initialize;
using PBGame.UI.Navigations.Screens.Initialize;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Skins;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class InitializeScreen : BaseScreen, IInitializeScreen {

        private IInitLoader initLoader;


        public ILogoDisplay LogoDisplay { get; private set; }

        public ILoadDisplay LoadDisplay { get; private set; }

        protected override int ScreenDepth => ViewDepths.InitializeScreen;

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init(IMapManager mapManager, ISkinManager skinManager)
        {
            Depth = 1000;

            // Initialize logo displayer.
            LogoDisplay = CreateChild<LogoDisplay>("logo", 10);
            {
                LogoDisplay.Size = new Vector2(320f, 320f);

                LogoDisplay.OnStartup = OnLogoStartup;
                LogoDisplay.OnEnd = OnLogoEnd;
            }

            // Initialize load displayer,
            LoadDisplay = CreateChild<LoadDisplay>("load", 9);
            {
                LoadDisplay.Anchor = Anchors.BottomStretch;
                LoadDisplay.OffsetLeft = 0f;
                LoadDisplay.OffsetRight = 0f;
                LoadDisplay.Y = 0f;
            }

            // Initialize loader
            initLoader = new InitLoader(mapManager, skinManager);
            initLoader.OnProgress += OnLoaderProgress;
            initLoader.OnStatusChange += OnLoaderStatus;
            initLoader.Load();

            // Display logo animation.
            LogoDisplay.PlayStartup();
        }

        /// <summary>
        /// Event called from logo display when startup animation has finished.
        /// </summary>
        private void OnLogoStartup()
        {
            // If all loading is already complete, skip over to logo end.
            if (initLoader.IsComplete)
                LogoDisplay.PlayEnd();
            // Else, loop breathing animation.
            else
                LogoDisplay.PlayBreathe();
        }

        /// <summary>
        /// Event called from logo display when end animation has finished.
        /// </summary>
        private void OnLogoEnd()
        {
            OverlayNavigator.Show<BackgroundOverlay>();
            ScreenNavigator.Show<HomeScreen>();
        }

        /// <summary>
        /// Event called from loader when the loader progress has changed.
        /// </summary>
        private void OnLoaderProgress(float progress)
        {
            LoadDisplay.SetProgress(progress);
        }

        /// <summary>
        /// EVent called from loader when the loader status has changed.
        /// </summary>
        private void OnLoaderStatus(string status)
        {
            LoadDisplay.SetStatus(status);
        }
    }
}