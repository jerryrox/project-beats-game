using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaMiscEntry : UguiObject, IHasLabel {

        private ILabel sectionLabel;
        private ILabel contentLabel;


        public string LabelText
        {
            get => sectionLabel.Text;
            set => sectionLabel.Text = value;
        }

        /// <summary>
        /// Text displayed as content.
        /// </summary>
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
                sectionLabel.Anchor = AnchorType.TopLeft;
                sectionLabel.Pivot = PivotType.TopLeft;
                sectionLabel.X = 32f;
                sectionLabel.Y = -32f;
                sectionLabel.Alignment = TextAnchor.UpperLeft;
                sectionLabel.IsBold = true;
                sectionLabel.FontSize = 18;
            }
            contentLabel = CreateChild<Label>("content", 1);
            {
                contentLabel.Anchor = AnchorType.Fill;
                contentLabel.Offset = new Offset(32f, 62f, 32f, 0f);
                contentLabel.Alignment = TextAnchor.UpperLeft;
                contentLabel.FontSize = 18;
                contentLabel.WrapText = true;
            }
        }
    }
}