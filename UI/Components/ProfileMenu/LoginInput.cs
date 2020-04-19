using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.Utils;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PBGame.UI.Components.ProfileMenu
{
    public class LoginInput : InputBox, IPointerClickHandler, IPointerDownHandler {

        public event Action OnFocus;
        public event Action OnUnfocus;

        private IAnime focusAni;
        private IAnime unfocusAni;


        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            component.onEndEdit.AddListener((value) => SetFocus(false));

            backgroundSprite.SpriteName = "circle-16";
            backgroundSprite.ImageType = Image.Type.Sliced;
            backgroundSprite.Color = Color.black;

            placeholderLabel.RawSize = valueLabel.RawSize = new Vector2(-16f, -16f);
            placeholderLabel.Alignment = valueLabel.Alignment = TextAnchor.MiddleCenter;
            placeholderLabel.FontSize = valueLabel.FontSize = 16;
            placeholderLabel.WrapText = valueLabel.WrapText = true;

            focusAni = new Anime();
            focusAni.AnimateColor(color => backgroundSprite.Color = color)
                .AddTime(0f, () => backgroundSprite.Color)
                .AddTime(0.25f, HexColor.Create("384852"))
                .Build();

            unfocusAni = new Anime();
            unfocusAni.AnimateColor(color => backgroundSprite.Color = color)
                .AddTime(0f, () => backgroundSprite.Color)
                .AddTime(0.25f, Color.black)
                .Build();
        }

        public void SetFocus(bool isFocused)
        {
            if (isFocused)
            {
                unfocusAni.Stop();
                focusAni.PlayFromStart();
            }
            else
            {
                focusAni.Stop();
                unfocusAni.PlayFromStart();
            }
        }

        /// <summary>
        /// Simulates invalid input value feedback to user.
        /// </summary>
        public void ShowInvalid()
        {
            var col = ColorPreset.Negative;
            col *= 0.125f;
            col.a = 1f;
            backgroundSprite.Color = col;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            SetFocus(true);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            SetFocus(true);
        }
    }
}