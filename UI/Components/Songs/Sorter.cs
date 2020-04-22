using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class Sorter : UguiObject {

        private const float ButtonSize = 80f;

        private ILabel label;

        private IGrid grid;
        private List<SortButton> sortButtons = new List<SortButton>();


        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = Anchors.LeftStretch;
                label.Pivot = Pivots.Left;
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
                grid.Anchor = Anchors.LeftStretch;
                grid.Pivot = Pivots.Left;
                grid.SetOffsetVertical(0f);
                grid.X = label.X * 2f + label.PreferredWidth;
                grid.Width = ButtonSize * Enum.GetNames(typeof(MapsetSorts)).Length;
                grid.CellSize = new Vector2(ButtonSize, 56f);
            }

            foreach (var sortType in (MapsetSorts[])Enum.GetValues(typeof(MapsetSorts)))
            {
                var button = grid.CreateChild<SortButton>(sortType.ToString(), sortButtons.Count);
                {
                    button.SortType = sortType;
                    button.LabelText = sortType.ToString();

                    button.OnTriggered += () => SetSort(button.SortType);
                }
                sortButtons.Add(button);
            }

            // Set initial selection
            SetSort(GameConfiguration.MapsetSort.Value);
        }

        /// <summary>
        /// Sets the sorting method of the mapsets.
        /// </summary>
        public void SetSort(MapsetSorts sort)
        {
            // Apply on button.
            for (int i = 0; i < sortButtons.Count; i++)
                sortButtons[i].IsFocused = sortButtons[i].SortType == sort;

            // Notify change
            OnSortChange(sort);
        }

        /// <summary>
        /// Event called when the current sort type has been changed.
        /// </summary>
        private void OnSortChange(MapsetSorts sort)
        {
            if (GameConfiguration.MapsetSort.Value != sort)
            {
                GameConfiguration.MapsetSort.Value = sort;
                GameConfiguration.Save();
            }
        }
    }
}