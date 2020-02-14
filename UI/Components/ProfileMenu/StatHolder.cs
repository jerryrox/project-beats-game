using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class StatHolder : UguiSprite, IStatHolder {

        private IStatDisplay levelDisplay;
        private IStatDisplay accuracyDisplay;

        // TODO: Integrate with user data.


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
        }
    }
}