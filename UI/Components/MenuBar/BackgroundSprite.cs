using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MenuBar
{
    public class BackgroundSprite : UguiObject, IHasColor {

        /// <summary>
        /// The speed of color interpolation.
        /// </summary>
        private const float ColorSpeed = 3f;

        /// <summary>
        /// The current background color to display.
        /// </summary>
        private Color curColor = Color.black;

        /// <summary>
        /// The last color of the background before change.
        /// </summary>
        private Color lastColor = Color.black;

        /// <summary>
        /// Interpolation time value.
        /// </summary>
        private float animateTime = 1f;


        /// <summary>
        /// Returns the background sprite instance.
        /// </summary>
        public ISprite Sprite { get; private set; }

        public Color Color
        {
            get => curColor;
            set
            {
                lastColor = Sprite.Color;
                curColor = value;
                enabled = true;
            }
        }

        public float Alpha
        {
            get => curColor.a;
            set
            {
                curColor.a = value;
                Color = curColor;
            }
        }


        [InitWithDependency]
        private void Init()
        {
            Sprite = CreateChild<UguiSprite>("sprite");
            {
                Sprite.Color = curColor;
                Sprite.Anchor = AnchorType.Fill;
                Sprite.RawSize = Vector2.zero;
            }
        }

        void Update()
        {
            animateTime += Time.deltaTime * ColorSpeed;
            if (animateTime >= 1f)
            {
                animateTime = 1f;
                enabled = false;
            }
            Sprite.Color = Color.Lerp(lastColor, curColor, animateTime);
        }
    }
}