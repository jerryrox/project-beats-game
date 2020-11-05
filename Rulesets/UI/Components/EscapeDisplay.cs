using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.UI.Components
{
    public class EscapeDisplay : UguiObject {

        private BindableBool isLeftEscapePressed = new BindableBool(false);
        private BindableBool isRightEscapePressed = new BindableBool(false);


        [ReceivesDependency]
        private GameModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            var leftTrigger = CreateChild<BasicTrigger>("left");
            {
                leftTrigger.Anchor = AnchorType.LeftStretch;
                leftTrigger.Pivot = PivotType.Left;
                leftTrigger.X = 0f;
                leftTrigger.Width = 8f;
                leftTrigger.SetOffsetVertical(0f);
                
                leftTrigger.OnPointerDown += OnLeftTriggerDown;
                leftTrigger.OnPointerUp += OnLeftTriggerUp;
            }
            var rightTrigger = CreateChild<BasicTrigger>("right");
            {
                rightTrigger.Anchor = AnchorType.RightStretch;
                rightTrigger.Pivot = PivotType.Right;
                rightTrigger.X = 0f;
                rightTrigger.Width = 8f;
                rightTrigger.SetOffsetVertical(0f);

                rightTrigger.OnPointerDown += OnRightTriggerDown;
                rightTrigger.OnPointerUp += OnRightTriggerUp;
            }

            OnEnableInited();
        }

        /// <summary>
        /// Manually triggers escape gesture.
        /// </summary>
        public void TriggerEscape()
        {
            isLeftEscapePressed.Value = false;
            isRightEscapePressed.Value = false;

            Model.CurrentSession?.InvokePause();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            isLeftEscapePressed.Value = false;
            isRightEscapePressed.Value = false;

            isLeftEscapePressed.Bind(TryTriggerEscape);
            isRightEscapePressed.Bind(TryTriggerEscape);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            isLeftEscapePressed.Unbind(TryTriggerEscape);
            isRightEscapePressed.Unbind(TryTriggerEscape);
        }

        /// <summary>
        /// Tries triggering escape if both escape triggers have been activated.
        /// </summary>
        private void TryTriggerEscape(bool _)
        {
            Debug.LogWarning($"Left: {isLeftEscapePressed.Value}, right: {isRightEscapePressed.Value}");
            if(isLeftEscapePressed.Value && isRightEscapePressed.Value)
                TriggerEscape();
        }

        /// <summary>
        /// Event called on left escape trigger down.
        /// </summary>
        private void OnLeftTriggerDown() => isLeftEscapePressed.Value = true;

        /// <summary>
        /// Event called on left escape trigger up.
        /// </summary>
        private void OnLeftTriggerUp() => isLeftEscapePressed.Value = false;

        /// <summary>
        /// Event called on right escape trigger down.
        /// </summary>
        private void OnRightTriggerDown() => isRightEscapePressed.Value = true;

        /// <summary>
        /// Event called on right escape trigger up.
        /// </summary>
        private void OnRightTriggerUp() => isRightEscapePressed.Value = false;
    }
}