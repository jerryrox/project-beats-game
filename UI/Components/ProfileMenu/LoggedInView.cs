using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class LoggedInView : UguiObject, IHasAlpha {

        private CanvasGroup canvasGroup;

        private CoverDisplay coverDisplay;
        private ISprite coverBackground;
        private ISprite shadow;
        private Header header;
        private StatHolder statHolder;
        private MenuHolder menuHolder;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = myObject.AddComponent<CanvasGroup>();

            coverDisplay = CreateChild<CoverDisplay>("cover", 0);
            {
                coverDisplay.Anchor = AnchorType.TopStretch;
                coverDisplay.Pivot = PivotType.Top;
                coverDisplay.SetOffsetHorizontal(0f);
                coverDisplay.Y = 0f;
                coverDisplay.Height = 106f;
            }
            header = CreateChild<Header>("header", 2);
            {
                header.Anchor = AnchorType.TopStretch;
                header.Pivot = PivotType.Top;
                header.RawWidth = 0f;
                header.Y = 0f;
                header.Height = 178f;
            }
            statHolder = CreateChild<StatHolder>("stat", 3);
            {
                statHolder.Anchor = AnchorType.TopStretch;
                statHolder.Pivot = PivotType.Top;
                statHolder.RawWidth = 0f;
                statHolder.Y = -178f;
                statHolder.Height = 110f;
            }
            menuHolder = CreateChild<MenuHolder>("menu", 4);
            {
                menuHolder.Anchor = AnchorType.TopStretch;
                menuHolder.Pivot = PivotType.Top;
                menuHolder.RawWidth = 0f;
                menuHolder.Y = -288f;
                menuHolder.Height = 192f;
            }
        }
    }
}