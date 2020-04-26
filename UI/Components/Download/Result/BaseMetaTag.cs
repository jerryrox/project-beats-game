using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Result
{
    public class BaseMetaTag : UguiSprite, IHasLabel {

        protected ILabel label;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }


        [InitWithDependency]
        private void Init()
        {
            SpriteName = "circle-16";
            ImageType = Image.Type.Sliced;
            Color = new Color(1f, 1f, 1f, 0.75f);

            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = Anchors.Fill;
                label.Offset = Offset.Zero;
                label.Color = Color.black;
                label.IsBold = true;
                label.FontSize = 16;
            }
        }
    }
}