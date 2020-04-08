using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.SettingsMenu.Navbars
{
    public class NavBar : UguiGrid {

        [InitWithDependency]
        private void Init()
        {
            Anchor = Anchors.RightStretch;
            Pivot = Pivots.Right;
            Width = 72f;
            RawHeight = 0f;
            Position = Vector2.zero;
            InvokeAfterFrames(1, () =>
            {
                // CellSize = new Vector2(72f, Height / );
            });
        }
    }
}