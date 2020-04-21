using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Assets.Fonts;
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
    public class BasicInput : UguiInputBox, IHasTint,
        IPointerClickHandler, IPointerDownHandler,
        IPointerEnterHandler, IPointerExitHandler
    {

        /// <summary>
        /// Event called on input focus change.
        /// </summary>
        public event Action<bool> OnFocused;

        protected ISprite hoverSprite;
        protected ISprite focusSprite;

        protected IAnime hoverInAni;
        protected IAnime hoverOutAni;

        protected IAnime focusAni;
        protected IAnime unfocusAni;

        private Color tint;


        /// <summary>
        /// Whether the input is currently focused.
        /// </summary>
        public bool IsFocused
        {
            get => component.isFocused;
            set => SetFocus(value);
        }

        public override Color Color
        {
            get => tint;
            set => Tint = value;
        }

        public override Color Tint
        {
            get => tint;
            set
            {
                hoverSprite.Tint = value;
                focusSprite.Tint = value;
            }
        }


        [InitWithDependency]
        private void Init(IFontManager fontManager, IColorPreset colorPreset)
        {
            tint = colorPreset.PrimaryFocus;

            component.onEndEdit.AddListener(value => SetFocus(false));

            // Assign default font.
            valueLabel.Font = placeholderLabel.Font = fontManager.DefaultFont;

            backgroundSprite.SpriteName = "circle-16";
            backgroundSprite.ImageType = Image.Type.Sliced;
            backgroundSprite.Color = new Color(0f, 0f, 0f, 0.5f);

            placeholderLabel.Offset = valueLabel.Offset = new Offset(16f, 0f);
            placeholderLabel.Alignment = valueLabel.Alignment = TextAnchor.MiddleLeft;
            placeholderLabel.FontSize = valueLabel.FontSize = 16;
            placeholderLabel.WrapText = valueLabel.WrapText = true;

            hoverSprite = CreateChild<UguiSprite>("hover", 3);
            {
                hoverSprite.Anchor = Anchors.Fill;
                hoverSprite.Offset = new Offset(-13.5f);
                hoverSprite.SpriteName = "glow-circle-16-x2";
                hoverSprite.ImageType = Image.Type.Sliced;
                hoverSprite.Color = tint;
                hoverSprite.Alpha = 0f;
            }
            focusSprite = CreateChild<UguiSprite>("focus", 4);
            {
                focusSprite.Anchor = Anchors.Fill;
                focusSprite.Offset = Offset.Zero;
                focusSprite.SpriteName = "outline-circle-16";
                focusSprite.ImageType = Image.Type.Sliced;
                focusSprite.Color = tint;
                focusSprite.Alpha = 0f;
            }
        }

        /// <summary>
        /// Creates default hover in/out animations for the input.
        /// </summary>
        public virtual void UseDefaultHoverAni()
        {
            hoverInAni = new Anime();
            hoverInAni.AnimateFloat(a => hoverSprite.Alpha = a)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat(a => hoverSprite.Alpha = a)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        /// <summary>
        /// Creates default focus/unfocus animations for the input.
        /// </summary>
        public virtual void UseDefaultFocusAni()
        {
            focusAni = new Anime();
            focusAni.AnimateFloat(a => focusSprite.Alpha = a)
                .AddTime(0f, () => focusSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            unfocusAni = new Anime();
            unfocusAni.AnimateFloat(a => focusSprite.Alpha = a)
                .AddTime(0f, () => focusSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        /// <summary>
        /// Assigns focus state of the input.
        /// </summary>
        protected void SetFocus(bool isFocused)
        {
            focusAni?.Stop();
            unfocusAni?.Stop();

            if (isFocused)
            {
                focusAni?.PlayFromStart();
                component.ActivateInputField();
            }
            else
            {
                unfocusAni?.PlayFromStart();
                component.DeactivateInputField();
            }

            OnFocused?.Invoke(isFocused);
        }

        /// <summary>
        /// Sets hovered state of the input.
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

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            SetFocus(true);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            SetFocus(true);
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