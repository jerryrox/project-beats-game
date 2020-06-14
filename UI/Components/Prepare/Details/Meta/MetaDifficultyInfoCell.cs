using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaDifficultyInfoCell : UguiObject, IHasIcon, IHasLabel, IHasTint {

        private ISprite icon;
        private ILabel label;


        public string IconName
        {
            get => icon.SpriteName;
            set => icon.SpriteName = value;
        }

        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        public Color Tint
        {
            get => icon.Color;
            set => icon.Color = value;
        }


        [InitWithDependency]
        private void Init()
        {
            icon = CreateChild<UguiSprite>("icon", 0);
            {
                icon.X = -24f;
                icon.Size = new Vector2(32f, 32f);
                icon.SpriteName = "icon-time";
            }
            label = CreateChild<Label>("label", 1);
            {
                label.Pivot = PivotType.Left;
                label.X = 0f;
                label.Alignment = TextAnchor.MiddleLeft;
                label.FontSize = 18;
            }
        }
    }
}