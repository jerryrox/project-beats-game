using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.UI.Components
{
    public class EscapeDisplay : UguiObject {

        private EscapeButton leftButton;
        private EscapeButton rightButton;


        [ReceivesDependency]
        private GameModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            leftButton = CreateChild<EscapeButton>("left");
            {
                leftButton.Anchor = AnchorType.LeftStretch;
                leftButton.SetSide(PivotType.Left);
                leftButton.SetOffsetVertical(0f);
            }
            rightButton = CreateChild<EscapeButton>("right");
            {
                rightButton.Anchor = AnchorType.RightStretch;
                rightButton.SetSide(PivotType.Right);
                rightButton.SetOffsetVertical(0f);
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            leftButton.IsTriggered.Bind(TryTriggerEscape);
            rightButton.IsTriggered.Bind(TryTriggerEscape);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            leftButton?.IsTriggered.Unbind(TryTriggerEscape);
            rightButton?.IsTriggered.Unbind(TryTriggerEscape);
        }

        /// <summary>
        /// Tries triggering escape if both escape triggers have been activated.
        /// </summary>
        private void TryTriggerEscape(bool _)
        {
            if(leftButton.IsTriggered.Value && rightButton.IsTriggered.Value)
                Model.CurrentSession?.InvokePause();
        }
    }
}