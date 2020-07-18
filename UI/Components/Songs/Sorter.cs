using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class Sorter : UguiObject
    {

        private const float ButtonSize = 80f;

        private ILabel label;

        private IGrid grid;
        private List<SortButton> sortButtons = new List<SortButton>();


        [ReceivesDependency]
        private SongsModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = AnchorType.LeftStretch;
                label.Pivot = PivotType.Left;
                label.X = 24;
                var offset = label.Offset;
                offset.Vertical = 0f;
                label.Offset = offset;
                label.IsBold = true;
                label.Alignment = TextAnchor.MiddleLeft;
                label.WrapText = false;
                label.Text = "Sort by";
            }
            grid = CreateChild<UguiGrid>("grid", 1);
            {
                grid.Anchor = AnchorType.LeftStretch;
                grid.Pivot = PivotType.Left;
                grid.SetOffsetVertical(0f);
                grid.X = label.X * 2f + label.PreferredWidth;
                grid.Width = ButtonSize * Enum.GetNames(typeof(MapsetSortType)).Length;
                grid.CellSize = new Vector2(ButtonSize, 56f);
            }

            foreach (var sortType in (MapsetSortType[])Enum.GetValues(typeof(MapsetSortType)))
            {
                var button = grid.CreateChild<SortButton>(sortType.ToString(), sortButtons.Count);
                {
                    button.SortType = sortType;
                    button.LabelText = sortType.ToString();

                    button.OnTriggered += () => Model.SetSort(button.SortType);
                }
                sortButtons.Add(button);
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.SortType.BindAndTrigger(OnSortTypeChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.SortType.OnNewValue -= OnSortTypeChange;
        }

        /// <summary>
        /// Event called from model when the sorting type has changed.
        /// </summary>
        private void OnSortTypeChange(MapsetSortType type)
        {
            for (int i = 0; i < sortButtons.Count; i++)
                sortButtons[i].IsFocused = sortButtons[i].SortType == type;
        }
    }
}