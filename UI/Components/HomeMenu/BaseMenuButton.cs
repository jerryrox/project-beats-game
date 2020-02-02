using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.HomeMenu
{
    public abstract class BaseMenuButton : UguiTrigger, IMenuButton {

        private IAnime hoverAni;
        private IAnime outAni;
        private IAnime pulseAni;

        private ISprite pulseSprite;


        public ISprite IconSprite { get; private set; }

        public ISprite FlashSprite { get; private set; }

        public ILabel Label { get; private set; }

        /// <summary>
        /// Returns the spritename of the icon.
        /// </summary>
        protected abstract string IconName { get; }

        /// <summary>
        /// Returns the text displayed on the label.
        /// </summary>
        protected abstract string LabelText { get; }


        [InitWithDependency]
        private void Init(ISoundPooler soundPooler)
        {
            OnPointerEnter += () =>
            {
                soundPooler.Play("menuhit");

                outAni.Stop();
                hoverAni.PlayFromStart();
            };
            OnPointerExit += () =>
            {
                hoverAni.Stop();
                outAni.PlayFromStart();
            };
            OnPointerClick += () =>
            {
                soundPooler.Play("menuclick");

                pulseAni.PlayFromStart();
            };

            IconSprite = CreateChild<UguiSprite>("icon", 2);
            {
                IconSprite.Size = new Vector2(64f, 64f);
                IconSprite.SpriteName = IconName;
            }
            pulseSprite = CreateChild<UguiSprite>("pulse-icon", 3);
            {
                pulseSprite.Size = new Vector2(64f, 64f);
                pulseSprite.SpriteName = IconName;
                pulseSprite.Alpha = 0f;
            }
            FlashSprite = CreateChild<UguiSprite>("flash", 0);
            {
                FlashSprite.Anchor = Anchors.Fill;
                FlashSprite.RawSize = Vector2.zero;
                FlashSprite.SpriteName = "glow-128";
                FlashSprite.Alpha = 0f;
            }
            Label = CreateChild<Label>("label", 1);
            {
                Label.Y = -52f;
                Label.IsBold = true;
                Label.FontSize = 24;
                Label.Text = LabelText;
            }

            hoverAni = new Anime();
            hoverAni.AnimateFloat((alpha) => FlashSprite.Alpha = alpha)
                .AddTime(0f, () => FlashSprite.Alpha)
                .AddTime(0.5f, 0.5f)
                .Build();

            outAni = new Anime();
            outAni.AnimateFloat((alpha) => FlashSprite.Alpha = alpha)
                .AddTime(0f, () => FlashSprite.Alpha)
                .AddTime(0.5f, 0f)
                .Build();

            pulseAni = new Anime();
            pulseAni.AnimateFloat((alpha) => pulseSprite.Alpha = alpha)
                .AddTime(0f, 0.5f, EaseType.QuartEaseIn)
                .AddTime(1f, 0f)
                .Build();
            pulseAni.AnimateVector3((scale) => pulseSprite.Scale = scale)
                .AddTime(0f, Vector3.one, EaseType.QuartEaseOut)
                .AddTime(1f, new Vector3(2.5f, 2.5f, 1f))
                .Build();
        }
    }
}