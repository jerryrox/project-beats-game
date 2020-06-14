using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common
{
    public class DialogButton : BoxButton, IHasLabel, IHasTint {

        private const float BaseWidth = 720f;
        private const float HoverWidth = 880f;

        private Color tint;


        public override Color Tint
        {
            get => tint;
            set => hoverSprite.Color = tint = value;
        }


        [InitWithDependency]
        private void Init(IRootMain root)
        {
            Size = new Vector2(BaseWidth, 56f);

            hoverSprite.ImageType = Image.Type.Sliced;
            hoverSprite.SpriteName = "parallel-64";
            hoverSprite.Alpha = 1f;

            label.FontSize = 20;
            label.Alpha = 1f;

            var resolution = root.Resolution;
            triggerAni = new Anime();
            triggerAni.AnimateFloat((x) => hoverSprite.Width = x)
                .AddTime(0f, () => hoverSprite.Width, EaseType.QuadEaseIn)
                .AddTime(0.25f, resolution.x * 1.2f)
                .Build();
            triggerAni.AnimateColor((color) => hoverSprite.Color = color)
                .AddTime(0f, () => hoverSprite.Color, EaseType.QuadEaseIn)
                .AddTime(0.05f, () => new Color(tint.r + 0.25f, tint.g + 0.25f, tint.b + 0.25f), EaseType.QuadEaseIn)
                .AddTime(0.35f, () => tint)
                .Build();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if(hoverSprite != null)
                hoverSprite.Width = BaseWidth;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (hoverSprite != null)
            {
                hoverSprite.Width = BaseWidth;
                hoverSprite.Color = tint;
            }
        }

        public override void UseDefaultHoverAni()
        {
            hoverInAni = new Anime();
            hoverInAni.AnimateFloat((x) => hoverSprite.Width = x)
                .AddTime(0f, () => hoverSprite.Width, EaseType.QuadEaseIn)
                .AddTime(0.25f, HoverWidth)
                .Build();
            hoverInAni.AnimateColor((color) => hoverSprite.Color = color)
                .AddTime(0f, () => hoverSprite.Color, EaseType.QuadEaseIn)
                .AddTime(0.25f, () => new Color(tint.r + 0.1f, tint.g + 0.1f, tint.b + 0.1f))
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat((x) => hoverSprite.Width = x)
                .AddTime(0f, () => hoverSprite.Width, EaseType.QuadEaseIn)
                .AddTime(0.25f, BaseWidth)
                .Build();
            hoverOutAni.AnimateColor((color) => hoverSprite.Color = color)
                .AddTime(0f, () => hoverSprite.Color, EaseType.QuadEaseIn)
                .AddTime(0.25f, () => tint)
                .Build();
        }

        protected override void OnPointerEntered()
        {
            if (triggerAni.IsPlaying)
                return;

            base.OnPointerEntered();
        }

        protected override void OnPointerExited()
        {
            if (triggerAni.IsPlaying)
                return;

            base.OnPointerExited();
        }

        protected override void OnClickTriggered()
        {
            if (triggerAni.IsPlaying)
                return;

            base.OnClickTriggered();
        }
    }
}