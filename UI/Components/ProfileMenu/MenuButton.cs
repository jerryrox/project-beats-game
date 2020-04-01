using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class MenuButton : ButtonTrigger, IMenuButton {

        protected ISprite bgSprite;
        protected ILabel label;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        public Color Tint
        {
            get => bgSprite.Color;
            set
            {
                value.a = bgSprite.Alpha;
                bgSprite.Color = value;
            }
        }


        [InitWithDependency]
        private void Init()
        {
            bgSprite = CreateChild<UguiSprite>("background", 0);
            {
                bgSprite.Anchor = Anchors.Fill;
                bgSprite.RawSize = Vector2.zero;
                bgSprite.Alpha = 0.35f;
            }
            label = CreateChild<Label>("label", 1);
            {
                label.Anchor = Anchors.Fill;
                label.RawSize = Vector2.zero;
                label.IsBold = true;
                label.Alignment = TextAnchor.MiddleCenter;
                label.WrapText = true;
                label.FontSize = 17;
                label.Alpha = 0.35f;
            }

            hoverAni = new Anime();
            hoverAni.AnimateFloat(alpha => label.Alpha = bgSprite.Alpha = alpha)
                .AddTime(0f, () => label.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            outAni = new Anime();
            outAni.AnimateFloat(alpha => label.Alpha = bgSprite.Alpha = alpha)
                .AddTime(0f, () => label.Alpha)
                .AddTime(0.25f, 0.35f)
                .Build();
        }
    }
}