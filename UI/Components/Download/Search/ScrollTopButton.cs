using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

namespace PBGame.UI.Components.Download.Search
{
    public class ScrollTopButton : HoverableTrigger, IHasAlpha {

        private CanvasGroup canvasGroup;

        private ISprite glowSprite;
        private UIGradient bgGradient;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        protected override int HoverSpriteDepth => 1;

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            IsClickToTrigger = true;

            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            glowSprite = CreateChild<UguiSprite>("glow", 0);
            {
                glowSprite.Anchor = AnchorType.Fill;
                glowSprite.Offset = new Offset(-14f);
                glowSprite.SpriteName = "glow-circle-16-x2";
                glowSprite.ImageType = Image.Type.Sliced;
                glowSprite.Color = ColorPreset.PrimaryFocus.Alpha(0.5f);
            }

            CreateIconSprite(depth: 2, spriteName: "icon-up", size: 20, alpha: 1f);

            hoverSprite.Color = ColorPreset.Passive.Alpha(0.75f);
            hoverSprite.SpriteName = "circle-16";
            hoverSprite.ImageType = Image.Type.Sliced;

            bgGradient = hoverSprite.AddEffect(new GradientEffect()).Component;
            bgGradient.direction = UIGradient.Direction.Vertical;
            bgGradient.color1 = new Color(0.75f, 0.75f, 0.75f, 1f);

            hoverInAni = new Anime();
            hoverInAni.AnimateFloat((alpha) => glowSprite.Alpha = alpha)
                .AddTime(0f, () => glowSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();
            hoverInAni.AnimateFloat((alpha) => hoverSprite.Alpha = alpha)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();
            
            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat((alpha) => glowSprite.Alpha = alpha)
                .AddTime(0f, () => glowSprite.Alpha)
                .AddTime(0.25f, 0.5f)
                .Build();
            hoverOutAni.AnimateFloat((alpha) => hoverSprite.Alpha = alpha)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0.75f)
                .Build();
        }
    }
}