using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// Slider customized for PB Game.
    /// </summary>
    public class BasicSlider : UguiSlider, IHasTint,
        IPointerEnterHandler, IPointerExitHandler
    {
        protected ISprite glowSprite;

        protected IAnime hoverInAni;
        protected IAnime hoverOutAni;

        private Color tint;


        public Color Tint
        {
            get => tint;
            set
            {
                glowSprite.Tint = value;
                foreground.Tint = value;
                thumb.Tint = value;
            }
        }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            tint = colorPreset.PrimaryFocus;

            CreateChild<Blocker>("blocker", -10);

            background.Anchor = AnchorType.MiddleStretch;
            background.Depth = -1;
            background.Height = 8f;
            background.SpriteName = "circle-8";
            background.ImageType = Image.Type.Sliced;
            background.Color = Color.black;

            glowSprite = CreateChild<UguiSprite>("glow", 0);
            {
                glowSprite.Anchor = AnchorType.MiddleStretch;
                glowSprite.SetOffsetHorizontal(-13.5f);
                glowSprite.Height = 35f;
                glowSprite.SpriteName = "glow-circle-8-x4";
                glowSprite.ImageType = Image.Type.Sliced;
                glowSprite.Color = tint;
                glowSprite.Alpha = 0f;
            }

            foregroundArea.Anchor = AnchorType.MiddleStretch;
            foregroundArea.Height = 8f;

            foreground.SpriteName = "circle-8";
            foreground.ImageType = Image.Type.Sliced;
            foreground.Color = tint;

            thumbArea.Anchor = AnchorType.MiddleStretch;
            thumbArea.Height = 24f;

            thumb.SpriteName = "circle-32";
            thumb.Size = new Vector2(24f, 24f);
            thumb.Color = tint;

            var thumbDark = thumb.CreateChild<UguiSprite>("dark", 0);
            {
                thumbDark.Anchor = AnchorType.Fill;
                thumbDark.Offset = new Offset(2.5f);
                thumbDark.Position = Vector2.zero;
                thumbDark.SpriteName = "circle-32";
                thumbDark.Color = Color.black;
            }

            UseDefaultHoverAni();
        }

        /// <summary>
        /// Creates default hover in/out animations.
        /// </summary>
        public virtual void UseDefaultHoverAni()
        {
            hoverInAni = new Anime();
            hoverInAni.AnimateFloat(a => glowSprite.Alpha = a)
                .AddTime(0f, () => glowSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat(a => glowSprite.Alpha = a)
                .AddTime(0f, () => glowSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        /// <summary>
        /// Assigns hover state of the slider.
        /// </summary>
        private void SetHover(bool isHovered)
        {
            hoverInAni?.Stop();
            hoverOutAni?.Stop();

            if(isHovered)
                hoverInAni?.PlayFromStart();
            else
                hoverOutAni?.PlayFromStart();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            SetHover(true);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            SetHover(false);
        }
    }
}