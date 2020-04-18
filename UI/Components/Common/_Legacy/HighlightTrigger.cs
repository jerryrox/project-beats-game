using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components
{
    public abstract class HighlightTrigger : ButtonTrigger, IHighlightTrigger {

        protected ILabel label;
        protected ISprite hoverSprite;
        protected ISprite highlightSprite;

        protected IAnime focusAni;
        protected IAnime unfocusAni;


        public virtual bool IsFocused { get; private set; }

        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        /// <summary>
        /// Returns the width of the highlight sprite when focused.
        /// </summary>
        protected abstract float HighlightWidth { get; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = Anchors.Fill;
                label.RawSize = Vector2.zero;
                label.IsBold = true;
                label.FontSize = 18;
            }
            hoverSprite = CreateChild<UguiSprite>("hover", 1);
            {
                hoverSprite.Anchor = Anchors.Fill;
                hoverSprite.RawSize = Vector2.zero;
                hoverSprite.Alpha = 0f;
            }
            highlightSprite = CreateChild<UguiSprite>("highlight", 2);
            {
                highlightSprite.Anchor = Anchors.Bottom;
                highlightSprite.Y = 0f;
                highlightSprite.Color = colorPreset.PrimaryFocus;
                highlightSprite.SpriteName = "glow-bar";
                highlightSprite.ImageType = Image.Type.Sliced;
                highlightSprite.Width = 0f;
                highlightSprite.Height = 20f;
            }

            hoverAni = new Anime();
            hoverAni.AnimateFloat(alpha => hoverSprite.Alpha = alpha)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0.25f)
                .Build();

            outAni = new Anime();
            outAni.AnimateFloat(alpha => hoverSprite.Alpha = alpha)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();

            focusAni = new Anime();
            focusAni.AnimateFloat(width => highlightSprite.Width = width)
                .AddTime(0f, () => highlightSprite.Width)
                .AddTime(0.3f, HighlightWidth)
                .Build();
            focusAni.AnimateFloat(alpha => highlightSprite.Alpha = alpha)
                .AddTime(0f, () => highlightSprite.Alpha)
                .AddTime(0.3f, 1f)
                .Build();

            unfocusAni = new Anime();
            unfocusAni.AnimateFloat(width => highlightSprite.Width = width)
                .AddTime(0f, () => highlightSprite.Width)
                .AddTime(0.3f, 0f)
                .Build();
            unfocusAni.AnimateFloat(alpha => highlightSprite.Alpha = alpha)
                .AddTime(0f, () => highlightSprite.Alpha)
                .AddTime(0.3f, 0f)
                .Build();
        }

        public void SetFocus(bool isFocused, bool animate = true)
        {
            focusAni.Stop();
            unfocusAni.Stop();

            IsFocused = isFocused;

            if (!animate)
            {
                if (isFocused)
                    highlightSprite.Width = HighlightWidth;
                else
                    highlightSprite.Width = 0f;
            }
            else
            {
                if (isFocused)
                    focusAni.PlayFromStart();
                else
                    unfocusAni.PlayFromStart();
            }
        }
    }
}