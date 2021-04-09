using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common
{
    public class LabelButton : HoverableTrigger, IHasLabel {

        private ILabel label;


        /// <summary>
        /// Returns the label on this button.
        /// </summary>
        public ILabel Label => label;

        public string LabelText
        {
            get => Label.Text;
            set => Label.Text = value;
        }


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 1);
            {
                label.Anchor = AnchorType.Fill;
                label.RawSize = Vector2.zero;
                label.Alignment = TextAnchor.MiddleCenter;
                label.WrapText = true;
                label.FontSize = 17;
            }
            UseDefaultHoverAni();
        }
    }
}