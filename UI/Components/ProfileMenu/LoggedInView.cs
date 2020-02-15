using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    public class LoggedInView : UguiObject, ILoggedInView {

        private CanvasGroup canvasGroup;

        private ICoverDisplay coverDisplay;
        private ISprite background;
        private ISprite shadow;
        private IHeader header;
        private IStatHolder statHolder;
        private IMenuHolder menuHolder;


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
                coverDisplay.Anchor = Anchors.TopStretch;
                coverDisplay.Pivot = Pivots.Top;
                coverDisplay.OffsetLeft = coverDisplay.OffsetRight = 0f;
                coverDisplay.Y = 0f;
                coverDisplay.Height = 106f;
            }
            background = CreateChild<UguiSprite>("bg", 1);
            {
                background.Anchor = Anchors.BottomStretch;
                background.Pivot = Pivots.Bottom;
                background.OffsetLeft = background.OffsetRight = 0f;
                background.Y = 0f;
                background.Height = 374f;
                background.Color = HexColor.Create("1D2126");

                shadow = background.CreateChild<UguiSprite>("shadow");
                {
                    shadow.Anchor = Anchors.TopStretch;
                    shadow.Pivot = Pivots.Bottom;
                    shadow.OffsetLeft = shadow.OffsetRight = 0f;
                    shadow.Y = 0f;
                    shadow.Height = 32f;
                    shadow.Color = new Color(0f, 0f, 0f, 0.5f);
                    shadow.SpriteName = "gradation-bottom";
                }
            }
            header = CreateChild<Header>("header", 2);
            {
                header.Anchor = Anchors.TopStretch;
                header.Pivot = Pivots.Top;
                header.RawWidth = 0f;
                header.Y = 0f;
                header.Height = 178f;
            }
            statHolder = CreateChild<StatHolder>("stat", 3);
            {
                statHolder.Anchor = Anchors.TopStretch;
                statHolder.Pivot = Pivots.Top;
                statHolder.RawWidth = 0f;
                statHolder.Y = -178f;
                statHolder.Height = 110f;
            }
            menuHolder = CreateChild<MenuHolder>("menu", 4);
            {
                menuHolder.Anchor = Anchors.TopStretch;
                menuHolder.Pivot = Pivots.Top;
                menuHolder.RawWidth = 0f;
                menuHolder.Y = -288f;
                menuHolder.Height = 192f;
            }
        }
    }
}