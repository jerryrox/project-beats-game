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
    public class ImageBackgroundDisplay : UguiObject, IBackgroundDisplay {

        private UguiTexture texture;
        private CanvasGroup canvasGroup;

        private IMapBackground background;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        public Color Color
        {
            get => texture.Color;
            set => texture.Color = value;
        }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

            texture = CreateChild<UguiTexture>("texture");
            {
                texture.Anchor = Anchors.Fill;
                texture.RawSize = Vector2.zero;
                texture.Active = false;
            }
        }

        public void MountBackground(IMapBackground background)
        {
            this.background = background;

            if (background != null)
            {
                // Render the texture.
                texture.Texture = background.Image;
                texture.Active = true;
                texture.FillTexture();
            }
        }

        public void UnmountBackground()
        {
            this.background = null;
            texture.Texture = null;
            texture.Active = true;
        }
    }
}