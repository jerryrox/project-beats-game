using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Dependencies;
using UnityEngine;
using Coffee.UIExtensions;

namespace PBGame.UI.Components.Songs
{
    public class Background : UguiObject {

        private BlurDisplay blurDisplay;
        private ISprite darkSprite;
        private ISprite brightenSprite;
        private ISprite shadeSprite;


        [ReceivesDependency]
        private IRootMain RootMain { get; set; }


        [InitWithDependency]
        private void Init()
        {
            blurDisplay = CreateChild<BlurDisplay>("blur", 0);
            {
                blurDisplay.Anchor = AnchorType.Fill;
                blurDisplay.RawSize = Vector2.zero;
            }
            darkSprite = CreateChild<UguiSprite>("dark", 1);
            {
                darkSprite.Anchor = AnchorType.Fill;
                darkSprite.RawSize = Vector2.zero;
                darkSprite.Color = new Color(0f, 0f, 0f, 0.75f);
            }
            brightenSprite = CreateChild<UguiSprite>("brighten", 2);
            {
                float size = Math.Max(RootMain.Resolution.x, RootMain.Resolution.y);
                brightenSprite.Size = new Vector2(size, size);
                brightenSprite.Color = Color.gray;
                brightenSprite.SpriteName = "glow-128";

                brightenSprite.AddEffect(new AdditiveShaderEffect());
            }
            shadeSprite = CreateChild<UguiSprite>("shade", 3);
            {
                shadeSprite.Anchor = AnchorType.TopRight;
                shadeSprite.Pivot = PivotType.TopRight;
                shadeSprite.RotationZ = -5f;
                shadeSprite.Color = new Color(1f, 1f, 1f, 0.25f);
                shadeSprite.X = -480f;
                shadeSprite.Y = 0f;

                var gradient = shadeSprite.AddEffect(new GradientEffect()).Component;
                gradient.color1 = Color.black;
                gradient.direction = UIGradient.Direction.Vertical;
            }

            AdjustShade();
        }

        /// <summary>
        /// Tweaks the shade sprite to make the shade perfectly fit within the screen.
        /// </summary>
        private void AdjustShade()
        {
            var screenSize = RootMain.Resolution;
            var leftSpace = screenSize.x + shadeSprite.X;
            var bottomSpace = screenSize.y;

            shadeSprite.Width = Mathf.Cos(5 * Mathf.Deg2Rad) * leftSpace;
            shadeSprite.Height = Mathf.Sqrt(leftSpace * leftSpace + bottomSpace * bottomSpace);
        }
    }
}