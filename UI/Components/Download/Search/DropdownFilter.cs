using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Common.Dropdown;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public class DropdownFilter : BaseFilter {

        private DropdownButton dropdown;
        private DropdownContext context;

        private IBindable bindable;


        [InitWithDependency]
        private void Init()
        {
            context = new DropdownContext();
            context.OnSelection += (data) =>
            {
                if(bindable != null && data != null && bindable.RawValue.ToString() != data.ExtraData.ToString())
                    bindable.RawValue = data.ExtraData;
            };

            dropdown = container.CreateChild<DropdownButton>("dropdown", 1);
            {
                dropdown.Anchor = AnchorType.Fill;
                dropdown.Offset = new Offset(106f, 12f, 16f, 12f);
                dropdown.BackgroundSprite.Color = new Color(1f, 1f, 1f, 0.125f);

                dropdown.Context = context;
                dropdown.UseAutoSelect = false;
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
        /// Initializes the filter for the specified bindable and enum type.
        /// </summary>
        public void Setup<T>(IBindable bindable)
            where T : Enum
        {
            // Reset previous context.
            context.Datas.Clear();
            context.SelectData(null);

            this.bindable = bindable;
            context.ImportFromEnum<T>((T)bindable.RawValue);

            BindEvents();
        }

        /// <summary>
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            if(bindable == null)
                return;

            bindable.BindAndTrigger(OnFilterValueChange);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            bindable.OnRawValueChanged -= OnFilterValueChange;
        }

        /// <summary>
        /// Event called when the value of the bindable data has changed.
        /// </summary>
        private void OnFilterValueChange(object data, object _)
        {
            dropdown.LabelText = data.ToString();
        }
    }
}