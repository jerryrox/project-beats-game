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
    public class CategorySearchFilter : BaseSearchFilter {

        private DropdownButton dropdown;
        private DropdownContext context;


        [ReceivesDependency]
        private DownloadState State { get; set; }


        [InitWithDependency]
        private void Init()
        {
            context = new DropdownContext();
            context.ImportFromEnum<MapCategoryType>(State.Category.Value);
            context.OnSelection += (data) =>
            {
                if(data != null && State.Category.RawValue.ToString() != data.ExtraData.ToString())
                    State.Category.RawValue = data.ExtraData;
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

            State.Category.BindAndTrigger(OnRankStateChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            State.Category.OnNewValue -= OnRankStateChange;
        }

        /// <summary>
        /// Event called on rank state filter change.
        /// </summary>
        private void OnRankStateChange(MapCategoryType status)
        {
            dropdown.LabelText = status.ToString();
        }
    }
}