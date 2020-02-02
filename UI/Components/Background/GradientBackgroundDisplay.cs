using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Dependencies;
using Coffee.UIExtensions;
using UnityEngine;

namespace PBGame.UI.Components.Background
{
    public class GradientBackgroundDisplay : UguiObject, IBackgroundDisplay
    {
        private UguiSprite sprite;
        private CanvasGroup canvasGroup;
        private UIGradient gradient;

        private IMapBackground background;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        public Color Color
        {
            get => sprite.Color;
            set => sprite.Color = value;
        }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

            sprite = CreateChild<UguiSprite>("sprite");
            {
                sprite.Anchor = Anchors.Fill;
                sprite.RawSize = Vector2.zero;

                var effect = sprite.AddEffect(new GradientEffect());
                {
                    gradient = effect.Component;
                    gradient.direction = UIGradient.Direction.Angle;
                    gradient.rotation = -30f;
                }
            }
        }

        public void MountBackground(IMapBackground background)
        {
            this.background = background;

            if (background != null)
            {
                gradient.color1 = background.GradientTop;
                gradient.color2 = background.GradientBottom;
            }
        }

        public void UnmountBackground()
        {
            this.background = null;

            gradient.color1 = Color.black;
            gradient.color2 = new Color(0.25f, 0.25f, 0.25f);
        }
    }
}