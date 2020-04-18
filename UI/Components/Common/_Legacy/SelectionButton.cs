using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components
{
    public class SelectionButton : ButtonTrigger, ISelectionButton {

        private const float BaseWidth = 720f;
        private const float HoverWidth = 880f;

        private ISprite bgSprite;
        private ILabel label;

        private Color backgroundColor;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        public Color BackgroundColor
        {
            get => backgroundColor;
            set => bgSprite.Color = backgroundColor = value;
        }


        [InitWithDependency]
        private void Init(IRootMain root)
        {
            Size = new Vector2(BaseWidth, 56f);

            bgSprite = CreateChild<UguiSprite>("bg", 0);
            {
                bgSprite.Anchor = Anchors.Fill;
                bgSprite.RawSize = Vector2.zero;
                bgSprite.ImageType = Image.Type.Sliced;
                bgSprite.SpriteName = "parallel-64";
            }
            label = CreateChild<Label>("label", 1);
            {
                label.Anchor = Anchors.Fill;
                label.RawSize = Vector2.zero;
                label.IsBold = true;
                label.FontSize = 20;
            }

            var resolution = root.Resolution;

            hoverAni = new Anime();
            hoverAni.AnimateFloat((x) => bgSprite.Width = x)
                .AddTime(0f, () => bgSprite.Width, EaseType.QuadEaseIn)
                .AddTime(0.25f, HoverWidth)
                .Build();
            hoverAni.AnimateColor((color) => bgSprite.Color = color)
                .AddTime(0f, () => bgSprite.Color, EaseType.QuadEaseIn)
                .AddTime(0.25f, () => new Color(backgroundColor.r + 0.1f, backgroundColor.g + 0.1f, backgroundColor.b + 0.1f))
                .Build();

            outAni = new Anime();
            outAni.AnimateFloat((x) => bgSprite.Width = x)
                .AddTime(0f, () => bgSprite.Width, EaseType.QuadEaseIn)
                .AddTime(0.25f, BaseWidth)
                .Build();
            outAni.AnimateColor((color) => bgSprite.Color = color)
                .AddTime(0f, () => bgSprite.Color, EaseType.QuadEaseIn)
                .AddTime(0.25f, () => backgroundColor)
                .Build();

            triggerAni = new Anime();
            triggerAni.AnimateFloat((x) => bgSprite.Width = x)
                .AddTime(0f, () => bgSprite.Width, EaseType.QuadEaseIn)
                .AddTime(0.25f, resolution.x * 1.2f)
                .Build();
            triggerAni.AnimateColor((color) => bgSprite.Color = color)
                .AddTime(0f, () => bgSprite.Color, EaseType.QuadEaseIn)
                .AddTime(0.05f, () => new Color(backgroundColor.r + 0.25f, backgroundColor.g + 0.25f, backgroundColor.b + 0.25f), EaseType.QuadEaseIn)
                .AddTime(0.35f, () => backgroundColor)
                .Build();
        }

        public void Dispose()
        {
            hoverAni.Stop();
            hoverAni = null;
            outAni.Stop();
            outAni = null;
            triggerAni.Stop();
            triggerAni = null;

            Destroy();
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