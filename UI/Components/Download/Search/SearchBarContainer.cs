using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public class SearchBarContainer : UguiSprite {

        private RankSearchFilter rankSearchFilter;
        private SortSearchFilter sortSearchFilter;
        private SearchBarFilter searchBarFilter;
        private AdvancedButton advancedButton;


        /// <summary>
        /// Returns the advanced button for toggling advance search options.
        /// </summary>
        public AdvancedButton AdvancedButton => advancedButton;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            Color = colorPreset.DarkBackground;

            rankSearchFilter = CreateChild<RankSearchFilter>("rank-search", 0);
            {
                rankSearchFilter.Anchor = Anchors.LeftStretch;
                rankSearchFilter.Pivot = Pivots.Left;
                rankSearchFilter.SetOffsetVertical(16f);
                rankSearchFilter.X = 16f;
                rankSearchFilter.Width = 240;
            }
            sortSearchFilter = CreateChild<SortSearchFilter>("sort-search", 1);
            {
                sortSearchFilter.Anchor = Anchors.LeftStretch;
                sortSearchFilter.Pivot = Pivots.Left;
                sortSearchFilter.SetOffsetVertical(16f);
                sortSearchFilter.X = rankSearchFilter.X + rankSearchFilter.Width + 16f;
                sortSearchFilter.Width = 295;
            }

            advancedButton = CreateChild<AdvancedButton>("advanced", 3);
            {
                advancedButton.Anchor = Anchors.RightStretch;
                advancedButton.Pivot = Pivots.Right;
                advancedButton.SetOffsetVertical(16f);
                advancedButton.X = -16f;
                advancedButton.Width = 150f;
            }
            searchBarFilter = CreateChild<SearchBarFilter>("search-bar", 2);
            {
                searchBarFilter.Anchor = Anchors.RightStretch;
                searchBarFilter.Pivot = Pivots.Right;
                searchBarFilter.SetOffsetVertical(16f);
                searchBarFilter.X = advancedButton.X - advancedButton.Width - 16f;
                searchBarFilter.Width = 420f;
            }
        }
    }
}