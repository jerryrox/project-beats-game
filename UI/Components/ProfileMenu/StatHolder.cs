using PBGame.UI.Models;
using PBGame.Data.Users;
using PBGame.Rulesets;
using PBFramework.UI;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class StatHolder : UguiSprite {

        private StatDisplay levelDisplay;
        private StatDisplay accuracyDisplay;


        [ReceivesDependency]
        private ProfileMenuModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Color = new Color(1f, 1f, 1f, 0.125f);

            levelDisplay = CreateChild<StatDisplay>("level", 0);
            {
                levelDisplay.X = -64f;
                levelDisplay.Y = 8f;
                levelDisplay.Size = new Vector2(64f, 64f);
                levelDisplay.Progress = 0f;
                levelDisplay.LabelText = "Level";
            }
            accuracyDisplay = CreateChild<StatDisplay>("accuracy", 1);
            {
                accuracyDisplay.X = 64f;
                accuracyDisplay.Y = 8f;
                accuracyDisplay.Size = new Vector2(64f, 64f);
                accuracyDisplay.Progress = 0;
                accuracyDisplay.LabelText = "Accuracy";
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.GameMode.OnNewValue += OnGameModeChange;
            Model.CurrentUser.OnNewValue += OnUserChange;

            SetupDisplays();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.GameMode.OnNewValue -= OnGameModeChange;
            Model.CurrentUser.OnNewValue -= OnUserChange;
        }

        /// <summary>
        /// Refreshes display for current model state.
        /// </summary>
        private void SetupDisplays()
        {
            var gameMode = Model.GameMode.Value;
            var user = Model.CurrentUser.Value;
            if(user == null)
                return;

            var stats = user.GetStatistics(gameMode);

            levelDisplay.Progress = stats.ExpProgress;
            levelDisplay.CenterText = stats.Level.ToString();

            accuracyDisplay.Progress = stats.Accuracy;
            var roundedAcc = ((int)(stats.Accuracy * 1000f)) / 1000f;
            accuracyDisplay.CenterText = roundedAcc.ToString("N1");
        }

        /// <summary>
        /// Event called when the selected game mode has been changed.
        /// </summary>
        private void OnGameModeChange(GameModeType gameMode) => SetupDisplays();

        /// <summary>
        /// Event called when the current user has changed.
        /// </summary>
        private void OnUserChange(IUser user) => SetupDisplays();
    }
}