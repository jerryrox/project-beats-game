using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class SearchBar : GlowInputBox, ISearchBar {

        private const float SearchDelayTime = 1f;

        private ISprite icon;

        private float searchDelay = 0f;


        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            OnChanged += OnSearchBarChanged;
            OnSubmitted += OnSearchBarSubmitted;

            ValueLabel.OffsetRight = PlaceholderLabel.OffsetRight = 40f;

            icon = CreateChild<UguiSprite>("icon", 5);
            {
                icon.Anchor = Anchors.RightStretch;
                icon.Pivot = Pivots.Right;
                icon.X = -8f;
                icon.OffsetTop = 8f;
                icon.OffsetBottom = 8f;
                icon.Width = icon.Height;
                icon.SpriteName = "icon-search";
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Text = MapManager.LastSearch;
        }

        /// <summary>
        /// Applies search term on the map manager.
        /// </summary>
        private void SetSearch(string value)
        {
            MapManager.Search(value);

            // If only a single result, select it.
            if(MapManager.DisplayedMapsets.Count == 1)
                MapSelection.SelectMapset(MapManager.DisplayedMapsets[0]);
        }

        /// <summary>
        /// Event called when the input content has changed.
        /// </summary>
        private void OnSearchBarChanged(string value)
        {
            // Make the search be applied after a delay for performance.
            searchDelay = SearchDelayTime;
        }

        /// <summary>
        /// Event called when the input has been submitted.
        /// </summary>
        private void OnSearchBarSubmitted(string value)
        {
            searchDelay = 0f;
            SetSearch(value);
        }

        private void Update()
        {
            if (searchDelay > 0f)
            {
                searchDelay -= Time.deltaTime;
                if (searchDelay <= 0f)
                    SetSearch(Text);
            }
        }
    }
}