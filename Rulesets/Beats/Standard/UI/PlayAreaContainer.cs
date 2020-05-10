using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class PlayAreaContainer : Rulesets.UI.PlayAreaContainer {

        private ISprite bgSprite;
        private ISprite hitBarSprite;
        private HitObjectHolder hitObjectHolder;
        private PlayAreaFader playAreaFader;


        /// <summary>
        /// Returns the Y position from the bottom of the play area where the note should be hit.
        /// </summary>
        public float HitPosition => 150f;

        /// <summary>
        /// Returns the Y position which the hit object fade starts from.
        /// </summary>
        public float FadeStartPos = 2000f;

        [ReceivesDependency]
        private IRoot3D Root3D { get; set; }


        [InitWithDependency]
        private void Init()
        {
            bgSprite = CreateChild<UguiSprite>("bg", 0);
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.Offset = Offset.Zero;
                bgSprite.SpriteName = "gradation-bottom";
                bgSprite.Color = new Color(0f, 0f, 0f, 0.75f);

                var shadow = bgSprite.CreateChild<UguiSprite>("shadow", 1);
                {
                    shadow.Anchor = AnchorType.Fill;
                    shadow.Offset = new Offset(-30f);
                    shadow.SpriteName = "glow-square-32-x2";
                    shadow.ImageType = Image.Type.Sliced;

                    var gradient = shadow.AddEffect(new GradientEffect()).Component;
                    gradient.direction = UIGradient.Direction.Vertical;
                    gradient.color1 = new Color(0f, 0f, 0f, 0f);
                    gradient.color2 = Color.black;
                }
            }
            hitBarSprite = CreateChild<UguiSprite>("hit-bar", 1);
            {
                hitBarSprite.Anchor = AnchorType.BottomStretch;
                hitBarSprite.SetOffsetHorizontal(0f);
                hitBarSprite.Y = HitPosition;
                hitBarSprite.Height = 34;
                hitBarSprite.SpriteName = "glow-bar";
                hitBarSprite.ImageType = Image.Type.Sliced;
            }
            hitObjectHolder = CreateChild<HitObjectHolder>("obj-holder", 2);
            {
                hitObjectHolder.Anchor = AnchorType.Bottom;
                hitObjectHolder.Pivot = PivotType.Bottom;
                hitObjectHolder.Y = 0f;
            }
            playAreaFader = CreateChild<PlayAreaFader>("fader", 3);
            {
                playAreaFader.Anchor = AnchorType.BottomStretch;
                playAreaFader.Pivot = PivotType.Bottom;
                playAreaFader.SetOffsetHorizontal(0f);
                playAreaFader.Y = FadeStartPos;
                playAreaFader.Height = 10000f;
            }
        }
    }
}