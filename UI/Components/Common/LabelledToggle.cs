using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    public class LabelledToggle : BasicToggle, IHasLabel {

        protected ILabel label;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        public TextAnchor LabelAnchor
        {
            get => label.Alignment;
            set => label.Alignment = value;
        }


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 1);
            {
                label.Alignment = TextAnchor.MiddleLeft;
                label.Anchor = AnchorType.Fill;
                label.FontSize = 16;
            }

            SetIconAnchor(AnchorType.Right);
        }

        protected override void SetIconAnchor(AnchorType anchor)
        {
            base.SetIconAnchor(anchor);

            float iconWidth = iconHolder.Width;
            float iconHeight = iconHolder.Height;

            switch (anchor)
            {
                case AnchorType.Bottom:
                case AnchorType.BottomLeft:
                case AnchorType.BottomRight:
                    label.Offset = new Offset(0f, 0f, 0f, iconHeight);
                    break;

                case AnchorType.Left:
                    label.Offset = new Offset(iconWidth, 0f, 0f, 0f);
                    break;

                case AnchorType.Right:
                    label.Offset = new Offset(0f, 0f, iconWidth, 0f);
                    break;

                case AnchorType.Top:
                case AnchorType.TopLeft:
                case AnchorType.TopRight:
                    label.Offset = new Offset(0f, iconHeight, 0f, 0f);
                    break;

                default:
                    throw new ArgumentException("Unsupported anchor type: " + anchor);
            }
        }
    }
}