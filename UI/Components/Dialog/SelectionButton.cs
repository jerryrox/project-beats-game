using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Dialog
{
    public class SelectionButton : HoverableTrigger, IHasLabel {

        private const float BaseWidth = 720f;
        private const float HoverWidth = 880f;

        private ILabel label;

        private Color backgroundColor;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        /// <summary>
        /// The color of the background sprite.
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => hoverSprite.Color = backgroundColor = value;
        }


        [InitWithDependency]
        private void Init(IRootMain root)
        {
            Size = new Vector2(BaseWidth, 56f);

            hoverSprite.Anchor = Anchors.Fill;
            hoverSprite.RawSize = Vector2.zero;
            hoverSprite.ImageType = Image.Type.Sliced;
            hoverSprite.SpriteName = "parallel-64";
                
            label = CreateChild<Label>("label", 1);
            {
                label.Anchor = Anchors.Fill;
                label.RawSize = Vector2.zero;
                label.IsBold = true;
                label.FontSize = 20;
            }

            var resolution = root.Resolution;

            hoverInAni = new Anime();
            hoverInAni.AnimateFloat((x) => hoverSprite.Width = x)
                .AddTime(0f, () => hoverSprite.Width, EaseType.QuadEaseIn)
                .AddTime(0.25f, HoverWidth)
                .Build();
            hoverInAni.AnimateColor((color) => hoverSprite.Color = color)
                .AddTime(0f, () => hoverSprite.Color, EaseType.QuadEaseIn)
                .AddTime(0.25f, () => new Color(backgroundColor.r + 0.1f, backgroundColor.g + 0.1f, backgroundColor.b + 0.1f))
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat((x) => hoverSprite.Width = x)
                .AddTime(0f, () => hoverSprite.Width, EaseType.QuadEaseIn)
                .AddTime(0.25f, BaseWidth)
                .Build();
            hoverOutAni.AnimateColor((color) => hoverSprite.Color = color)
                .AddTime(0f, () => hoverSprite.Color, EaseType.QuadEaseIn)
                .AddTime(0.25f, () => backgroundColor)
                .Build();

            triggerAni = new Anime();
            triggerAni.AnimateFloat((x) => hoverSprite.Width = x)
                .AddTime(0f, () => hoverSprite.Width, EaseType.QuadEaseIn)
                .AddTime(0.25f, resolution.x * 1.2f)
                .Build();
            triggerAni.AnimateColor((color) => hoverSprite.Color = color)
                .AddTime(0f, () => hoverSprite.Color, EaseType.QuadEaseIn)
                .AddTime(0.05f, () => new Color(backgroundColor.r + 0.25f, backgroundColor.g + 0.25f, backgroundColor.b + 0.25f), EaseType.QuadEaseIn)
                .AddTime(0.35f, () => backgroundColor)
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