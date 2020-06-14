using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Data.Users;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class StatHolder : UguiSprite {

        private StatDisplay levelDisplay;
        private StatDisplay accuracyDisplay;

        // TODO: Integrate with user data.
        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


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

            SetupDisplays();
        }

        private void SetupDisplays()
        {
            var gameMode = GameConfiguration.RulesetMode.Value;
            var user = UserManager.CurrentUser.Value;
            if(user == null)
                return;

            var stats = user.GetStatistics(gameMode);

            levelDisplay.Progress = stats.ExpProgress;
            levelDisplay.CenterText = stats.Level.ToString();

            accuracyDisplay.Progress = stats.Accuracy;
            var roundedAcc = ((int)(stats.Accuracy * 1000f)) / 1000f;
            accuracyDisplay.CenterText = roundedAcc.ToString("N1");
        }
    }
}