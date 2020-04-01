using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components
{
    public class BoxIconTrigger : ButtonTrigger, IBoxIconTrigger {

        protected ISprite iconSprite;
        protected ISprite hoverSprite;


        public string IconName
        {
            get => iconSprite.SpriteName;
            set => iconSprite.SpriteName = value;
        }

        protected override bool IsClickToTrigger => false;


        [InitWithDependency]
        private void Init()
        {
            iconSprite = CreateChild<UguiSprite>("icon", 0);
            {
                iconSprite.Width = iconSprite.Height = 36f;
                iconSprite.Alpha = 0.65f;
            }
            hoverSprite = CreateChild<UguiSprite>("hover", 2);
            {
                hoverSprite.Anchor = Anchors.Fill;
                hoverSprite.RawSize = Vector2.zero;
                hoverSprite.Alpha = 0f;
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
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            hoverAni.Stop();
            outAni.Stop();

            hoverSprite.Alpha = 0f;
        }
    }
}