using PBGame.UI.Components.Common;
using PBGame.Audio;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MusicMenu
{
    public class ControlButton : HoverableTrigger {

        /// <summary>
        /// The size of the icon.
        /// </summary>
        public float IconSize
        {
            get => iconSprite.Width;
            set => iconSprite.Size = new Vector2(value, value);
        }


        [InitWithDependency]
        private void Init()
        {
            CreateIconSprite(depth: 0, alpha: 0.5f);

            // Remove useless sprite.
            hoverSprite.Destroy();

            hoverInAni = new Anime();
            hoverInAni.AnimateFloat(alpha => iconSprite.Alpha = alpha)
                .AddTime(0f, () => iconSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat(alpha => iconSprite.Alpha = alpha)
                .AddTime(0f, () => iconSprite.Alpha)
                .AddTime(0.25f, 0.5f)
                .Build();
        }
    }
}