using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public class ShadowButton : BasicTrigger, IHasAlpha {

        private ISprite shadowSprite;


        public float Alpha
        {
            get => shadowSprite.Alpha;
            set => shadowSprite.Alpha = value;
        }


        [InitWithDependency]
        private void Init()
        {
            shadowSprite = CreateChild<UguiSprite>("shadow", 1);
            {
                shadowSprite.Anchor = Anchors.Fill;
                shadowSprite.Offset = Offset.Zero;
                shadowSprite.SpriteName = "gradation-top";
                shadowSprite.Color = new Color(0f, 0f, 0f, 0.5f);
            }
        }
    }
}