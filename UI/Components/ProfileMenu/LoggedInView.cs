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
        private ISprite background;
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
            background = CreateChild<UguiSprite>("bg", 1);
            {
                background.Anchor = AnchorType.BottomStretch;
                background.Pivot = PivotType.Bottom;
                background.SetOffsetHorizontal(0f);
                background.Y = 0f;
                background.Height = 374f;
                background.Color = HexColor.Create("1D2126");

                shadow = background.CreateChild<UguiSprite>("shadow");
                {
                    shadow.Anchor = AnchorType.TopStretch;
                    shadow.Pivot = PivotType.Bottom;
                    shadow.SetOffsetHorizontal(0f);
                    shadow.Y = 0f;
                    shadow.Height = 32f;
                    shadow.Color = new Color(0f, 0f, 0f, 0.5f);
                    shadow.SpriteName = "gradation-bottom";
                }
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