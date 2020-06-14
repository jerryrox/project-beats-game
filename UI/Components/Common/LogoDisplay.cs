using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components
{
    public class LogoDisplay : UguiObject, ILogoDisplay {

        private CanvasGroup canvasGroup;


        public ISprite Glow { get; private set; }

        public ISprite Outer { get; private set; }

        public ISprite Inner { get; private set; }

        public ISprite Title { get; private set; }

        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

            Glow = CreateChild<UguiSprite>("glow");
            {
                Glow.SpriteName = "logo-outer-glow";
                Glow.Anchor = AnchorType.Fill;
                Glow.RawSize = Vector2.zero;
            }
            Outer = CreateChild<UguiSprite>("outer", 1);
            {
                Outer.SpriteName = "logo-outer";
                Outer.RawTransform.anchorMin = new Vector2(0.114726f, 0.114726f);
                Outer.RawTransform.anchorMax = new Vector2(0.885274f, 0.885274f);
                Outer.RawSize = Vector2.zero;
            }
            Inner = CreateChild<UguiSprite>("inner", 2);
            {
                Inner.SpriteName = "logo-inner";
                Inner.RawTransform.anchorMin = new Vector2(0.1532534f, 0.1532534f);
                Inner.RawTransform.anchorMax = new Vector2(0.8467466f, 0.8467466f);
                Inner.RawSize = Vector2.zero;
            }
            Title = CreateChild<UguiSprite>("title", 3);
            {
                Title.SpriteName = "logo-title";
                Title.RawTransform.anchorMin = new Vector2(0.2148973f, 0.3493151f);
                Title.RawTransform.anchorMax = new Vector2(0.7851027f, 0.650685f);
                Title.RawSize = Vector2.zero;
            }
        }
    }
}