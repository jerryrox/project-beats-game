using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaDifficultyScale : UguiObject, IHasTint {

        private ILabel label;
        private ILabel valueLabel;
        private IProgressBar progress;


        public Color Tint
        {
            get => label.Color;
            set => label.Color = progress.Foreground.Color = value;
        }


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Pivot = Pivots.Right;
                label.X = -5f;
                label.IsBold = true;
                label.FontSize = 18;
                label.Alignment = TextAnchor.MiddleRight;
            }
            valueLabel = CreateChild<Label>("value", 1);
            {
                valueLabel.Pivot = Pivots.Left;
                valueLabel.X = 5f;
                valueLabel.FontSize = 18;
                valueLabel.Alignment = TextAnchor.MiddleLeft;
            }
            progress = CreateChild<UguiProgressBar>("progress", 2);
            {
                progress.Anchor = Anchors.BottomStretch;
                progress.Pivot = Pivots.Bottom;
                progress.OffsetLeft = progress.OffsetRight = 0f;
                progress.Y = 0f;
                progress.Height = 2f;

                progress.Background.Color = Color.black;
            }
        }

        /// <summary>
        /// Sets specified values for display.
        /// </summary>
        public void Setup(string label, float value, float maxValue)
        {
            this.label.Text = label;
            valueLabel.Text = value.ToString("N2");
            progress.Value = value / maxValue;
        }
    }
}