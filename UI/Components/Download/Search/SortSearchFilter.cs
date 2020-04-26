using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public class SortSearchFilter : BaseSearchFilter
    {
        private DropdownButton dropdown;
        private DropdownContext context;
        private LabelledToggle toggle;


        [ReceivesDependency]
        private DownloadState State { get; set; }


        [InitWithDependency]
        private void Init()
        {
            context = new DropdownContext();
            context.ImportFromEnum<MapSortType>(State.Sort.Value);
            context.OnSelection += (data) =>
            {
                if(data != null && State.Sort.RawValue.ToString() != data.ExtraData.ToString())
                    State.Sort.RawValue = data.ExtraData;
            };

            label.Text = "Sort by";

            dropdown = CreateChild<DropdownButton>("dropdown", 1);
            {
                dropdown.Anchor = Anchors.LeftStretch;
                dropdown.Pivot = Pivots.Left;
                dropdown.X = 0f;
                dropdown.SetOffsetVertical(24f, 0f);
                dropdown.Width = 140f;
                dropdown.BackgroundSprite.Color = new Color(1f, 1f, 1f, 0.25f);

                dropdown.Context = context;
                dropdown.UseAutoSelect = false;
            }
            toggle = CreateChild<LabelledToggle>("toggle", 2);
            {
                toggle.Anchor = Anchors.Fill;
                toggle.Offset = new Offset(156f, 24f, 0f, 0f);

                toggle.LabelText = "Descending";

                toggle.OnTriggered += () => State.IsDescending.Value = !State.IsDescending.Value;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            State.IsDescending.BindAndTrigger(OnIsDescendingChange);
            State.Sort.BindAndTrigger(OnSortChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            State.IsDescending.OnValueChanged -= OnIsDescendingChange;
            State.Sort.OnValueChanged -= OnSortChange;
        }

        /// <summary>
        /// Event called on isDescending flag change.
        /// </summary>
        private void OnIsDescendingChange(bool isDescending, bool _)
        {
            toggle.IsFocused = isDescending;
        }

        /// <summary>
        /// Event called on map sort criteria change.
        /// </summary>
        private void OnSortChange(MapSortType type, MapSortType _)
        {
            dropdown.LabelText = type.ToString();
        }
    }
}