using System;
using PBGame.Graphics;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine.UI;

namespace PBGame.UI.Components
{
    public class GlowInputBox : InputBox, IInputBox {

        public event Action OnFocus;

        public event Action OnUnfocus;

        private IAnime focusAni;
        private IAnime unfocusAni;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            Background.Anchor = Anchors.BottomStretch;
            Background.Y = 0f;
            Background.OffsetLeft = -12f;
            Background.OffsetRight = -12f;
            Background.Height = 20f;
            Background.Color = colorPreset.PrimaryFocus;
            Background.Alpha = 0.25f;
            Background.SpriteName = "glow-bar";
            Background.ImageType = Image.Type.Sliced;

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
    }
}