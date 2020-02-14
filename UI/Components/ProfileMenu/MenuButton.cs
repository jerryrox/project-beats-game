using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class MenuButton : UguiTrigger, IMenuButton {

        private ISprite background;
        private ILabel label;

        private IAnime hoverAni;
        private IAnime outAni;


        public Color Tint
        {
            get => background.Color;
            set
            {
                value.a = background.Alpha;
                background.Color = value;
            }
        }

        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }


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
            };

            background = CreateChild<UguiSprite>("background", 0);
            {
                background.Anchor = Anchors.Fill;
                background.RawSize = Vector2.zero;
                background.Alpha = 0.35f;
            }
            label = CreateChild<Label>("label", 1);
            {
                label.Anchor = Anchors.Fill;
                label.RawSize = Vector2.zero;
                label.IsBold = true;
                label.Alignment = TextAnchor.MiddleCenter;
                label.WrapText = true;
                label.FontSize = 17;
            }

            hoverAni = new Anime();
            hoverAni.AnimateFloat(alpha => label.Alpha = background.Alpha = alpha)
                .AddTime(0f, () => label.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            outAni = new Anime();
            outAni.AnimateFloat(alpha => label.Alpha = background.Alpha = alpha)
                .AddTime(0f, () => label.Alpha)
                .AddTime(0.25f, 0.35f)
                .Build();
        }
    }
}