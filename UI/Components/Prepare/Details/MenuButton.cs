using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details
{
    public class MenuButton : HoverableTrigger, IHasLabel {

        private ILabel label;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }


        [InitWithDependency]
        private void Init()
        {
            CreateIconSprite().X = -36f;

            label = CreateChild<Label>("label", 2);
            {
                label.Pivot = Pivots.Left;
                label.Size = Vector2.zero;
                label.X = 0f;
                label.Alignment = TextAnchor.MiddleLeft;
            }

            UseDefaultHoverAni();
        }
    }
}