using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaMiscEntry : UguiObject, IMetaMiscEntry {

        private ILabel sectionLabel;
        private ILabel contentLabel;


        public string LabelText
        {
            get => sectionLabel.Text;
            set => sectionLabel.Text = value;
        }

        public string Content
        {
            get => contentLabel.Text;
            set => contentLabel.Text = value;
        }


        [InitWithDependency]
        private void Init()
        {
            sectionLabel = CreateChild<Label>("section", 0);
            {
                sectionLabel.Anchor = Anchors.TopLeft;
                sectionLabel.Pivot = Pivots.TopLeft;
                sectionLabel.X = 32f;
                sectionLabel.Y = -32f;
                sectionLabel.Alignment = TextAnchor.UpperLeft;
                sectionLabel.IsBold = true;
                sectionLabel.FontSize = 18;
            }
            contentLabel = CreateChild<Label>("content", 1);
            {
                contentLabel.Anchor = Anchors.Fill;
                contentLabel.OffsetLeft = contentLabel.OffsetRight = 32f;
                contentLabel.OffsetBottom = 0f;
                contentLabel.OffsetTop = 62f;
                contentLabel.Alignment = TextAnchor.UpperLeft;
                contentLabel.FontSize = 18;
                contentLabel.WrapText = true;
            }
        }
    }
}