using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.QuickMenu
{
    public abstract class BaseMenuButton : HighlightableTrigger, IHasLabel
    {
        protected Action triggerAction;

        private ILabel label;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }


        [InitWithDependency]
        private void Init()
        {
            IsClickToTrigger = true;
            OnTriggered += () =>
            {
                if(!IsFocused)
                    triggerAction?.Invoke();
            };

            var icon = CreateIconSprite(depth: 4, size: 32f);
            {
                icon.Y = 8f;
            }

            label = CreateChild<Label>("label", 5);
            {
                label.Anchor = AnchorType.MiddleStretch;
                label.SetOffsetHorizontal(0f);
                label.FontSize = 16;
                label.IsBold = true;
                label.Y = -20f;
            }

            UseDefaultFocusAni();
            UseDefaultHighlightAni();
            UseDefaultHoverAni();

            OnEnableInited();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // Remove hovered state.
            hoverOutAni.Seek(hoverOutAni.Duration);
        }

        /// <summary>
        /// Event called from quick menu overlay when the button should be shown.
        /// </summary>
        public abstract void OnShowQuickMenu();
    }
}