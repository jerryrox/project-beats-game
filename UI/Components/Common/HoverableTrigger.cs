using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// Trigger with assumed hover in/out visual interaction.
    /// </summary>
    public class HoverableTrigger : BasicTrigger {

        protected ISprite hoverSprite;

        protected IAnime hoverInAni;
        protected IAnime hoverOutAni;


        /// <summary>
        /// Returns the depth of the hover sprite.
        /// </summary>
        protected virtual int HoverSpriteDepth => 0;


        [InitWithDependency]
        private void Init()
        {
            hoverSprite = CreateChild<UguiSprite>("hover", HoverSpriteDepth);
            {
                hoverSprite.Anchor = Anchors.Fill;
                hoverSprite.RawSize = Vector2.zero;
                hoverSprite.Position = Vector2.zero;
                hoverSprite.Alpha = 0f;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            hoverInAni?.Stop();
            hoverOutAni?.Stop();
        }

        /// <summary>
        /// Creates default hover in/out animations.
        /// </summary>
        public virtual void UseDefaultHoverAni()
        {
            hoverInAni = new Anime();
            hoverInAni.AnimateFloat(a => hoverSprite.Alpha = a)
                .AddTime(0, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0.25f)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat(a => hoverSprite.Alpha = a)
                .AddTime(0, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        protected override void OnPointerEntered()
        {
            base.OnPointerEntered();

            if(hoverInAni != null)
                hoverInAni.PlayFromStart();
        }

        protected override void OnPointerExited()
        {
            base.OnPointerExited();

            if(hoverOutAni != null)
                hoverOutAni.PlayFromStart();
        }
    }
}