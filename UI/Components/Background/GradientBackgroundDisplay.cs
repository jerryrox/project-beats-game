using PBGame.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Dependencies;
using Coffee.UIExtensions;
using UnityEngine;

namespace PBGame.UI.Components.Background
{
    public class GradientBackgroundDisplay : BaseBackgroundDisplay, IBackgroundDisplay
    {
        private UguiSprite sprite;
        private UIGradient gradient;


        public override Color Color
        {
            get => sprite.Color;
            set => sprite.Color = value;
        }


        [InitWithDependency]
        private void Init()
        {
            sprite = CreateChild<UguiSprite>("sprite");
            {
                sprite.Anchor = Anchors.Fill;
                sprite.RawSize = Vector2.zero;

                var effect = sprite.AddEffect(new GradientEffect());
                {
                    gradient = effect.Component;
                    gradient.direction = UIGradient.Direction.Angle;
                    gradient.rotation = -30f;
                }
            }
        }

        public override void MountBackground(IMapBackground background)
        {
            base.MountBackground(background);

            gradient.color1 = background.GradientTop;
            gradient.color2 = background.GradientBottom;
        }
    }
}