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
    public class InitializeScreen : BaseScreen<InitializeModel> {

        private LogoDisplay logoDisplay;
        private LoadDisplay loadDisplay;


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
            logoDisplay = CreateChild<LogoDisplay>("logo", 10);
            {
                logoDisplay.Size = new Vector2(320f, 320f);

                logoDisplay.OnStartup = OnLogoStartup;
                logoDisplay.OnEnd = model.NavigateToNext;
            }

            // Initialize load displayer,
            loadDisplay = CreateChild<LoadDisplay>("load", 9);
            {
                loadDisplay.Anchor = AnchorType.BottomStretch;
                loadDisplay.SetOffsetHorizontal(0f);
                loadDisplay.Y = 0f;
            }

            // Hook state changes in model
            model.Progress.OnNewValue += OnLoaderProgress;
            model.State.OnNewValue += OnLoaderStatus;
            model.IsComplete.OnNewValue += OnLoaderComplete;

            // Display logo animation.
            logoDisplay.PlayStartup();

            // Start loading process for dependencies.
            model.StartLoad();
        }

        /// <summary>
        /// Event called from logo display when startup animation has finished.
        /// </summary>
        private void OnLogoStartup()
        {
            // If all loading is already complete, skip over to logo end.
            if (model.IsComplete.Value)
                logoDisplay.PlayEnd();
            // Else, loop breathing animation.
            else
                logoDisplay.PlayBreathe();
        }

        /// <summary>
        /// Event called from loader when the loader progress has changed.
        /// </summary>
        private void OnLoaderProgress(float progress)
        {
            loadDisplay.SetProgress(progress);
        }

        /// <summary>
        /// EVent called from loader when the loader status has changed.
        /// </summary>
        private void OnLoaderStatus(string status)
        {
            loadDisplay.SetStatus(status);
        }

        /// <summary>
        /// Event called from loader when the loading completion has changed.
        /// </summary>
        private void OnLoaderComplete(bool isComplete)
        {
            logoDisplay.PlayEnd();
        }
    }
}