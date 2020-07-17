using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.Dialog;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Rulesets;
using PBGame.Graphics;
using PBFramework.UI.Navigations;
using PBFramework.Data;
using PBFramework.Dependencies;

namespace PBGame.UI.Models
{
    public class PauseModel : BaseModel {

        private List<DialogOption> options;

        private DialogModel dialogModel;


        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        public PauseModel()
        {
            options = new List<DialogOption>()
            {
                new DialogOption()
                {
                    Label = "Resume",
                    Color = ColorPreset.Positive,
                    Callback = OnResumeOption
                },
                new DialogOption()
                {
                    Label = "Offset",
                    Color = ColorPreset.Passive,
                    Callback = OnOffsetOption
                },
                new DialogOption()
                {
                    Label = "Retry",
                    Color = ColorPreset.Warning,
                    Callback = OnRetryOption
                },
                new DialogOption()
                {
                    Label = "Quit",
                    Color = ColorPreset.Negative,
                    Callback = OnQuitOption
                }
            };
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            ShowDialogOverlay();
            SetupDialog();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            dialogModel = null;
        }

        /// <summary>
        /// Shows the dialog overlay.
        /// </summary>
        private void ShowDialogOverlay()
        {
            var overlay = OverlayNavigator.Show<DialogOverlay>();
            // Hide the pause overlay when the dialog closes.
            overlay.OnHide += OnDialogOverlayHide;

            dialogModel = overlay.Model;
        }

        /// <summary>
        /// Sets up the dialog options and message.
        /// </summary>
        private void SetupDialog()
        {
            if(dialogModel == null)
                return;

            dialogModel.SetMessage("Game paused");
            dialogModel.AddOptions(options);
        }

        /// <summary>
        /// Closes the pause overlay.
        /// </summary>
        private void CloseOverlay()
        {
            OverlayNavigator.Hide<PauseOverlay>();
        }

        /// <summary>
        /// Returns the current game session available in the game.
        /// </summary>
        private IGameSession GetGameSession()
        {
            var game = ScreenNavigator.Get<GameScreen>();
            if(game == null)
                return null;
            // TODO: This should be retrieved from game model.
            return game.CurSession;
        }

        /// <summary>
        /// Event called on selecting resume option.
        /// </summary>
        private void OnResumeOption()
        {
            GetGameSession()?.InvokeResume();
        }

        /// <summary>
        /// Event called on selecting offset option.
        /// </summary>
        private void OnOffsetOption()
        {
            // Show the offset overlay.
            OffsetsOverlay overlay = OverlayNavigator.Show<OffsetsOverlay>();

            // Bind temporary hide event listener to overlay.
            EventBinder<Action> closeBinder = new EventBinder<Action>(
                e => overlay.OnHide += e,
                e => overlay.OnHide -= e
            );
            closeBinder.IsOneTime = true;

            // Show pause overlay once the offset overlay has been closed.
            closeBinder.SetHandler(() => OverlayNavigator.Show<PauseOverlay>());
        }

        /// <summary>
        /// Event called on selecting retry option.
        /// </summary>
        private void OnRetryOption()
        {
            GetGameSession()?.InvokeRetry();
        }

        /// <summary>
        /// Event called on selecting quit option.
        /// </summary>
        private void OnQuitOption()
        {
            GetGameSession()?.InvokeForceQuit();
        }

        /// <summary>
        /// Event called on dialog overlay hiding.
        /// </summary>
        private void OnDialogOverlayHide()
        {
            CloseOverlay();
        }
    }
}