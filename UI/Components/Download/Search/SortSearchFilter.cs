using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.Networking.Maps;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Download.Search
{
    public class SortSearchFilter : BaseSearchFilter
    {
        private DropdownButton dropdown;
        private DropdownContext context;
        private LabelledToggle toggle;


        [ReceivesDependency]
        private DownloadModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            context = new DropdownContext();
            context.ImportFromEnum<MapSortType>(Model.Options.Sort.Value);
            context.OnSelection += (data) =>
            {
                var sort = Model.Options.Sort;
                if(data != null && !sort.RawValue.ToString().Equals(data.ExtraData.ToString()))
                    sort.RawValue = data.ExtraData;
            };

            label.Text = "Sort by";

            dropdown = CreateChild<DropdownButton>("dropdown", 1);
            {
                dropdown.Anchor = AnchorType.LeftStretch;
                dropdown.Pivot = PivotType.Left;
                dropdown.X = 0f;
                dropdown.SetOffsetVertical(24f, 0f);
                dropdown.Width = 140f;
                dropdown.BackgroundSprite.Color = new Color(1f, 1f, 1f, 0.25f);

                dropdown.Context = context;
                dropdown.UseAutoSelect = false;
            }
            toggle = CreateChild<LabelledToggle>("toggle", 2);
            {
                toggle.Anchor = AnchorType.Fill;
                toggle.Offset = new Offset(156f, 24f, 0f, 0f);

                toggle.LabelText = "Descending";

                toggle.OnTriggered += Model.Options.ToggleIsDescending;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.Options.IsDescending.BindAndTrigger(OnIsDescendingChange);
            Model.Options.Sort.BindAndTrigger(OnSortChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Options.IsDescending.OnNewValue -= OnIsDescendingChange;
            Model.Options.Sort.OnNewValue -= OnSortChange;
        }

        /// <summary>
        /// Event called on isDescending flag change.
        /// </summary>
        private void OnIsDescendingChange(bool isDescending)
        {
            toggle.IsFocused = isDescending;
        }

        /// <summary>
        /// Event called on map sort criteria change.
        /// </summary>
        private void OnSortChange(MapSortType type)
        {
            dropdown.LabelText = type.ToString();

            // Making sure the dropdown selection is synchronized with the option.
            context.SelectDataWithText(type.ToString());
        }
    }
}