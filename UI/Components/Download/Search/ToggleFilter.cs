using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBFramework.Data.Bindables;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public class ToggleFilter : BaseFilter {

        private BasicToggle toggle;
        private UguiSprite touchArea;

        private BindableBool bindable;


        [InitWithDependency]
        private void Init()
        {
            toggle = container.CreateChild<BasicToggle>("toggle", 1);
            {
                toggle.Anchor = Anchors.Right;
                toggle.Pivot = Pivots.Right;
                toggle.X = -16f;
                toggle.Size = new Vector2(36f, 16f);

                toggle.OnTriggered += () =>
                {
                    if(bindable != null)
                        bindable.Value = !bindable.Value;
                };
            }
            touchArea = container.CreateChild<UguiSprite>("touch-area", 5);
            {
                touchArea.Anchor = Anchors.Fill;
                touchArea.Alpha = 0f;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            BindEvents();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            UnbindEvents();
        }

        /// <summary>
        /// Initializes the filter to toggle specified bindable data.
        /// </summary>
        public void Setup(BindableBool bindable)
        {
            AdjustTouchArea();

            this.bindable = bindable;
            BindEvents();
        }

        /// <summary>
        /// Adjusts touch area size so it fits this object.
        /// </summary>
        private void AdjustTouchArea()
        {
            // Making sure the area is fully covered correctly.
            InvokeAfterTransformed(5, () =>
            {
                touchArea.SetParent(container);
                touchArea.Offset = Offset.Zero;
                touchArea.SetParent(toggle);
            });
        }

        /// <summary>
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            if(bindable == null)
                return;

            bindable.BindAndTrigger(OnBoolValueChange);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            bindable.OnValueChanged -= OnBoolValueChange;
        }

        /// <summary>
        /// Event called when the boolean value of bindable has changed.
        /// </summary>
        private void OnBoolValueChange(bool value, bool _)
        {
            toggle.IsFocused = value;
        }
    }
}