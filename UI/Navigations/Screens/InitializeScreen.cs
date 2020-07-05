using PBGame.UI.Models;
using PBGame.UI.Components.Initialize;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class InitializeScreen : BaseScreen<InitializeModel>, IInitializeScreen {

        public LogoDisplay LogoDisplay { get; private set; }

        public LoadDisplay LoadDisplay { get; private set; }

        protected override int ViewDepth => ViewDepths.InitializeScreen;

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Depth = 1000;

            // Initialize logo displayer.
            LogoDisplay = CreateChild<LogoDisplay>("logo", 10);
            {
                LogoDisplay.Size = new Vector2(320f, 320f);

                LogoDisplay.OnStartup = OnLogoStartup;
                LogoDisplay.OnEnd = model.NavigateToNext;
            }

            // Initialize load displayer,
            LoadDisplay = CreateChild<LoadDisplay>("load", 9);
            {
                LoadDisplay.Anchor = AnchorType.BottomStretch;
                LoadDisplay.SetOffsetHorizontal(0f);
                LoadDisplay.Y = 0f;
            }

            // Hook state changes in model
            model.Progress.OnNewValue += OnLoaderProgress;
            model.State.OnNewValue += OnLoaderStatus;
            model.IsComplete.OnNewValue += OnLoaderComplete;

            // Display logo animation.
            LogoDisplay.PlayStartup();

            // Start loading process for dependencies.
            model.StartLoad();
        }

        protected override InitializeModel CreateModel() => new InitializeModel();

        /// <summary>
        /// Event called from logo display when startup animation has finished.
        /// </summary>
        private void OnLogoStartup()
        {
            // If all loading is already complete, skip over to logo end.
            if (model.IsComplete.Value)
                LogoDisplay.PlayEnd();
            // Else, loop breathing animation.
            else
                LogoDisplay.PlayBreathe();
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

        /// <summary>
        /// Event called from loader when the loading completion has changed.
        /// </summary>
        private void OnLoaderComplete(bool isComplete)
        {
            LogoDisplay.PlayEnd();
        }
    }
}