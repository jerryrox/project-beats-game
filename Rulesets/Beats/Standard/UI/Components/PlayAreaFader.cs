using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public class PlayAreaFader : UguiObject {

        /// <summary>
        /// Returns the height of the gradation.
        /// </summary>
        public float FadeSize = 2200;


        [InitWithDependency]
        private void Init()
        {
            var fill = CreateChild<UguiSprite>("fill", 0);
            {
                fill.Anchor = AnchorType.Fill;
                fill.Offset = new Offset(0f, 0f, 0f, FadeSize);
                fill.Color = Color.black;
            }
            var fade = CreateChild<UguiSprite>("fade", 1);
            {
                fade.Anchor = AnchorType.BottomStretch;
                fade.Pivot = PivotType.Bottom;
                fade.SetOffsetHorizontal(0f);
                fade.Y = 0f;
                fade.Height = FadeSize;
                fade.SpriteName = "gradation-top";
                fade.Color = Color.black;
            }
        }
    }
}