using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public abstract class BaseFilter : UguiObject, IHasLabel {

        protected UguiSprite container;
        protected ILabel titleLabel;


        public string LabelText
        {
            get => titleLabel.Text;
            set => titleLabel.Text = value;
        }


        [InitWithDependency]
        private void Init()
        {
            container = CreateChild<UguiSprite>("container", 0);
            {
                container.Anchor = AnchorType.Fill;
                container.Offset = new Offset(16f, 0f);
                container.SpriteName = "circle-16";
                container.ImageType = Image.Type.Sliced;
                container.Color = new Color(1f, 1f, 1f, 0.25f);

                titleLabel = container.CreateChild<Label>("title", 0);
                {
                    titleLabel.Anchor = AnchorType.LeftStretch;
                    titleLabel.Pivot = PivotType.Left;
                    titleLabel.SetOffsetVertical(0f);
                    titleLabel.X = 16f;
                    titleLabel.IsBold = true;
                    titleLabel.FontSize = 17;
                    titleLabel.Alignment = TextAnchor.MiddleLeft;
                }
            }
        }
    }
}