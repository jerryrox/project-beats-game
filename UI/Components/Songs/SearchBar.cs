using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine.UI;

namespace PBGame.UI.Components.Songs
{
    public class SearchBar : InputBox, ISearchBar {

        private ISprite icon;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
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
        }
    }
}