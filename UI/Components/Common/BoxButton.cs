using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// HoverableTrigger which assumes a button with a colorable background and button label.
    /// </summary>
    public class BoxButton : HoverableTrigger, IHasColor, IHasLabel {

        private const float HoveredInAlpha = 1f;
        private const float HoveredOutAlpha = 0.4f;

        protected ILabel label;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        public Color Color
        {
            get => hoverSprite.Color;
            set
            {
                value.a = hoverSprite.Alpha;
                hoverSprite.Color = value;
            }
        }

        public float Alpha
        {
            get => hoverSprite.Alpha;
            set => label.Alpha = hoverSprite.Alpha = value;
        }


        [InitWithDependency]
        private void Init()
        {
            hoverSprite.Alpha = HoveredOutAlpha;

            label = CreateChild<Label>("label", 1);
            {
                label.Anchor = Anchors.Fill;
                label.RawSize = Vector2.zero;
                label.IsBold = true;
                label.Alignment = TextAnchor.MiddleCenter;
                label.WrapText = true;
                label.FontSize = 17;
                label.Alpha = HoveredOutAlpha;
            }

            UseDefaultHoverAni();
        }

        public override void UseDefaultHoverAni()
        {
            hoverInAni = new Anime();
            hoverInAni.AnimateFloat(alpha => this.Alpha = alpha)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, HoveredInAlpha)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat(alpha => this.Alpha = alpha)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, HoveredOutAlpha)
                .Build();
        }
    }
}