using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Background
{
    public class GradientBackgroundDisplay : UguiObject, IBackgroundDisplay
    {
        private UguiSprite sprite;
        private CanvasGroup canvasGroup;

        private IMapBackground background;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

            sprite = CreateChild<UguiSprite>("sprite");
            {
                sprite.Anchor = Anchors.Fill;
                sprite.RawSize = Vector2.zero;
            }
        }

        public void MountBackground(IMapBackground background)
        {
            this.background = background;

            if (background != null)
            {
                // TODO: Apply gradient.

            }
        }

        public void UnmountBackground()
        {
            this.background = null;
        }
    }
}