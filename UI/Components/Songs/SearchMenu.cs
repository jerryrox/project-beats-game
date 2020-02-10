using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class SearchMenu : UguiObject, ISearchMenu {

        private ISprite backgroundSprite;


        public ISorter Sorter { get; private set; }

        public ISearchBar SearchBar { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            backgroundSprite = CreateChild<UguiSprite>("background", 0);
            {
                backgroundSprite.Anchor = Anchors.Fill;
                backgroundSprite.RawSize = Vector2.zero;
                backgroundSprite.Color = new Color(1f, 1f, 1f, 0.125f);
            }
            Sorter = CreateChild<Sorter>("sorter", 1);
            {
                Sorter.Anchor = Anchors.LeftStretch;
                Sorter.Pivot = Pivots.Left;
                Sorter.X = 0f;
                Sorter.OffsetTop = 0f;
                Sorter.OffsetBottom = 0f;
            }
            SearchBar = CreateChild<SearchBar>("search-bar", 2);
            {
                SearchBar.Anchor = Anchors.RightStretch;
                SearchBar.Pivot = Pivots.Right;
                SearchBar.X = -32f;
                SearchBar.Width = 420f;
                SearchBar.OffsetTop = 3f;
                SearchBar.OffsetBottom = 15f;
            }
        }
    }
}