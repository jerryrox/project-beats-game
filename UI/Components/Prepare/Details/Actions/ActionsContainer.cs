using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Prepare.Details.Actions
{
    public class ActionsContainer : UguiSprite {

        [ReceivesDependency]
        private PrepareModel Model { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            Alpha = 0f;

            var actionButton = CreateChild<BoxButton>("actions");
            {
                actionButton.Anchor = AnchorType.Fill;
                actionButton.Offset = Offset.Zero;
                actionButton.Tint = colorPreset.Passive;
                actionButton.LabelText = "Map Actions";

                actionButton.OnTriggered += () => Model.ShowMapActions(Model.SelectedMap.Value?.OriginalMap);
            }
        }
    }
}