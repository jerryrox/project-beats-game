using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common
{
    public class BasicScrollbar : UguiScrollBar {

        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            background.Color = Color.black;
            background.SpriteName = "circle-8";
            background.ImageType = Image.Type.Sliced;

            foreground.Color = colorPreset.PrimaryFocus;
            foreground.SpriteName = "circle-8";
            foreground.ImageType = Image.Type.Sliced;

            SetNoTransition();
            SetVertical();
        }

        /// <summary>
        /// Configures the scrollbar for vertical scrolling.
        /// </summary>
        public void SetVertical()
        {
            Direction = Scrollbar.Direction.BottomToTop;
        }

        /// <summary>
        /// Configures the scrollbar for horizontal scrolling.
        /// </summary>
        public void SetHorizontal()
        {
            Direction = Scrollbar.Direction.RightToLeft;
        }
    }
}