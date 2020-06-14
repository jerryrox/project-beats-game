using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class SearchMenu : UguiObject {

        private ISprite backgroundSprite;


        /// <summary>
        /// Returns the mapset sorter object.
        /// </summary>
        public Sorter Sorter { get; private set; }

        /// <summary>
        /// Returns the search bar.
        /// </summary>
        public SearchBar SearchBar { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            backgroundSprite = CreateChild<UguiSprite>("background", 0);
            {
                backgroundSprite.Anchor = AnchorType.Fill;
                backgroundSprite.RawSize = Vector2.zero;
                backgroundSprite.Color = new Color(1f, 1f, 1f, 0.125f);
            }
            Sorter = CreateChild<Sorter>("sorter", 1);
            {
                Sorter.Anchor = AnchorType.LeftStretch;
                Sorter.Pivot = PivotType.Left;
                Sorter.X = 0f;
                Sorter.SetOffsetVertical(0f);
            }
            SearchBar = CreateChild<SearchBar>("search-bar", 2);
            {
                SearchBar.Anchor = AnchorType.RightStretch;
                SearchBar.Pivot = PivotType.Right;
                SearchBar.X = -32f;
                SearchBar.Width = 420f;
                SearchBar.SetOffsetVertical(0f);
            }
        }
    }
}