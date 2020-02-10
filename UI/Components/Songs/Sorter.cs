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
    public class Sorter : UguiObject, ISorter {

        private const float ButtonSize = 80f;

        private ILabel label;

        private IGrid grid;
        private List<ISortButton> sortButtons = new List<ISortButton>();


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
                label.OffsetTop = 0f;
                label.OffsetBottom = 0f;
                label.IsBold = true;
                label.Alignment = TextAnchor.MiddleLeft;
                label.WrapText = false;
                label.Text = "Sort by";
            }
            grid = CreateChild<UguiGrid>("grid", 1);
            {
                grid.Anchor = Anchors.LeftStretch;
                grid.Pivot = Pivots.Left;
                grid.OffsetTop = 0f;
                grid.OffsetBottom = 0f;
                grid.X = 100f;
                grid.Width = ButtonSize * Enum.GetNames(typeof(MapsetSorts)).Length;
                grid.CellSize = new Vector2(ButtonSize, 56f);
            }

            foreach (var sortType in (MapsetSorts[])Enum.GetValues(typeof(MapsetSorts)))
            {
                var button = grid.CreateChild<SortButton>(sortType.ToString(), sortButtons.Count);
                {
                    button.SortType = sortType;
                    button.LabelText = sortType.ToString();

                    button.OnPointerDown += () => SetSort(button.SortType);
                }
                sortButtons.Add(button);
            }

            // Set initial selection
            SetSort(GameConfiguration.MapsetSort.Value);
        }

        public void SetSort(MapsetSorts sort)
        {
            // Apply on button.
            for (int i = 0; i < sortButtons.Count; i++)
            {
                sortButtons[i].SetToggle(sortButtons[i].SortType == sort);
            }

            // Notify change
            OnSortChange(sort);
        }

        /// <summary>
        /// Event called when the current sort type has been changed.
        /// </summary>
        private void OnSortChange(MapsetSorts sort)
        {
            GameConfiguration.MapsetSort.Value = sort;
        }
    }
}