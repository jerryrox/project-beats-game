using System;
using PBGame.Graphics;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// BasicInput implementation with assumption of a glowing bar at the bottom of the input.
    /// </summary>
    public class GlowInput : BasicInput
    {

        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            backgroundSprite.Sprite = null;
            backgroundSprite.Anchor = Anchors.BottomStretch;
            backgroundSprite.Y = 2f;
            backgroundSprite.Height = 4f;
            backgroundSprite.Alpha = 0.75f;

            hoverSprite.Offset = Offset.Zero;
            hoverSprite.Sprite = null;
            hoverSprite.Tint = Color.white;

            focusSprite.SpriteName = "glow-bar";
            focusSprite.Anchor = Anchors.BottomStretch;
            focusSprite.SetOffsetHorizontal(-15f);
            focusSprite.Y = 2f;
            focusSprite.Height = 34f;

            valueLabel.SetOffsetLeft(8f);
            placeholderLabel.SetOffsetLeft(8f);

            UseDefaultFocusAni();

            hoverInAni = new Anime();
            hoverInAni.AnimateFloat(a => hoverSprite.Alpha = a)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0.25f)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat(a => hoverSprite.Alpha = a)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();

            focusAni.AnimateFloat((w) => focusSprite.Width = w)
                .AddTime(0f, () => focusSprite.Width)
                .AddTime(0.25f, () => Width + 30f)
                .Build();

            unfocusAni.AnimateFloat((w) => focusSprite.Width = w)
                .AddTime(0f, () => focusSprite.Width)
                .AddTime(0.25f, 0f)
                .Build();
        }
    }
}