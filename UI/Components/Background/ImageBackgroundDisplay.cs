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
    public class ImageBackgroundDisplay : BaseBackgroundDisplay, IBackgroundDisplay {

        private UguiTexture texture;


        public override Color Color
        {
            get => texture.Color;
            set => texture.Color = value;
        }


        [InitWithDependency]
        private void Init()
        {
            texture = CreateChild<UguiTexture>("texture");
            {
                texture.Anchor = Anchors.Fill;
                texture.RawSize = Vector2.zero;
                texture.Active = false;
            }
        }

        public override void MountBackground(IMapBackground background)
        {
            base.MountBackground(background);

            texture.Texture = background.Image;
            if (background.Image == null)
            {
                texture.Active = false;
            }
            else
            {
                texture.Active = true;
                texture.FillTexture();
            }
        }
    }
}