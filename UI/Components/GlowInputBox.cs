using System;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PBGame.UI.Components
{
    public class GlowInputBox : InputBox, IInputBox, IPointerClickHandler, IPointerDownHandler
    {

        public event Action OnFocus;

        public event Action OnUnfocus;

        private IAnime focusAni;
        private IAnime unfocusAni;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            // Since the existing backgrond sprite IS on the root input box itself, we should override this.
            backgroundSprite.SpriteName = "null";
            backgroundSprite = CreateChild<UguiSprite>("bg", 0);
            {
                backgroundSprite.Anchor = Anchors.Fill;
                backgroundSprite.RawSize = Vector2.zero;
            }

            backgroundSprite.Anchor = Anchors.BottomStretch;
            backgroundSprite.Y = 0f;
            backgroundSprite.OffsetLeft = -12f;
            backgroundSprite.OffsetRight = -12f;
            backgroundSprite.Height = 20f;
            backgroundSprite.Color = colorPreset.PrimaryFocus;
            backgroundSprite.Alpha = 0.25f;
            backgroundSprite.SpriteName = "glow-bar";
            backgroundSprite.ImageType = Image.Type.Sliced;

            ValueLabel.OffsetLeft = PlaceholderLabel.OffsetLeft = 4f;
            ValueLabel.OffsetRight = PlaceholderLabel.OffsetRight = 4f;
            ValueLabel.FontSize = PlaceholderLabel.FontSize = 18;
            ValueLabel.WrapText = PlaceholderLabel.WrapText = true;

            component.onEndEdit.AddListener((value) => SetFocus(false));

            focusAni = new Anime();
            focusAni.AnimateFloat((alpha) => Background.Alpha = alpha)
                .AddTime(0f, () => Background.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            unfocusAni = new Anime();
            unfocusAni.AnimateFloat((alpha) => Background.Alpha = alpha)
                .AddTime(0f, () => Background.Alpha)
                .AddTime(0.25f, 0.25f)
                .Build();
        }

        public virtual void SetFocus(bool isFocused)
        {
            if (isFocused)
            {
                OnFocus?.Invoke();

                unfocusAni.Stop();
                focusAni.PlayFromStart();
            }
            else
            {
                OnUnfocus?.Invoke();

                focusAni.Stop();
                unfocusAni.PlayFromStart();
            }
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