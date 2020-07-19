using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.Networking.Maps;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Download.Search
{
    public class CategorySearchFilter : BaseSearchFilter {

        private DropdownButton dropdown;
        private DropdownContext context;


        [ReceivesDependency]
        private DownloadModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            context = new DropdownContext();
            context.ImportFromEnum<MapCategoryType>(Model.Options.Category.Value);
            context.OnSelection += (data) =>
            {
                var category = Model.Options.Category;
                if(data != null && !category.RawValue.ToString().Equals(data.ExtraData.ToString()))
                    category.RawValue = data.ExtraData;
            };

            label.Text = "Rank state";

            dropdown = CreateChild<DropdownButton>("dropdown", 1);
            {
                dropdown.Anchor = AnchorType.Fill;
                dropdown.Offset = new Offset(0f, 24f, 0f, 0f);
                dropdown.BackgroundSprite.Color = new Color(1f, 1f, 1f, 0.25f);

                dropdown.Context = context;
                dropdown.UseAutoSelect = false;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.Options.Category.BindAndTrigger(OnRankStateChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Options.Category.OnNewValue -= OnRankStateChange;
        }

        /// <summary>
        /// Event called on rank state filter change.
        /// </summary>
        private void OnRankStateChange(MapCategoryType category)
        {
            dropdown.LabelText = category.ToString();

            // Making sure the dropdown selection is synchronized with the option.
            context.SelectDataWithText(category.ToString());
        }
    }
}