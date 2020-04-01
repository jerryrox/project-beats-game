using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details
{
    public class MenuButton : BoxIconTrigger, IMenuButton {

        private ILabel label;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }


        [InitWithDependency]
        private void Init()
        {
            iconSprite.X = -36f;
            iconSprite.Size = new Vector2(36f, 36f);

            label = CreateChild<Label>("label", 2);
            {
                label.Pivot = Pivots.Left;
                label.Size = Vector2.zero;
                label.X = 0f;
                label.Alignment = TextAnchor.MiddleLeft;
            }
        }
    }
}