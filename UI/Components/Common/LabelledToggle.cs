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
                label.Anchor = Anchors.Fill;
                label.FontSize = 16;
            }

            SetIconAnchor(Anchors.Right);
        }

        protected override void SetIconAnchor(Anchors anchor)
        {
            base.SetIconAnchor(anchor);

            float iconWidth = iconHolder.Width;
            float iconHeight = iconHolder.Height;

            switch (anchor)
            {
                case Anchors.Bottom:
                case Anchors.BottomLeft:
                case Anchors.BottomRight:
                    label.Offset = new Offset(0f, 0f, 0f, iconHeight);
                    break;

                case Anchors.Left:
                    label.Offset = new Offset(iconWidth, 0f, 0f, 0f);
                    break;

                case Anchors.Right:
                    label.Offset = new Offset(0f, 0f, iconWidth, 0f);
                    break;

                case Anchors.Top:
                case Anchors.TopLeft:
                case Anchors.TopRight:
                    label.Offset = new Offset(0f, iconHeight, 0f, 0f);
                    break;

                default:
                    throw new ArgumentException("Unsupported anchor type: " + anchor);
            }
        }
    }
}