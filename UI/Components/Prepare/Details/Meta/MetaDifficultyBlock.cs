using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaDifficultyBlock : UguiSprite, IHasTint {

        private ILabel label;
        private ILabel valueLabel;


        public new Color Tint
        {
            get => label.Tint;
            set => label.Tint = value;
        }


        [InitWithDependency]
        private void Init()
        {
            Color = new Color(1f, 1f, 1f, 0.0625f);

            label = CreateChild<Label>("label", 0);
            {
                label.Pivot = PivotType.Right;
                label.X = -5f;
                label.IsBold = true;
                label.FontSize = 18;
                label.Alignment = TextAnchor.MiddleRight;
            }
            valueLabel = CreateChild<Label>("value", 1);
            {
                valueLabel.Pivot = PivotType.Left;
                valueLabel.X = 5f;
                valueLabel.FontSize = 18;
                valueLabel.Alignment = TextAnchor.MiddleLeft;
            }
        }

        /// <summary>
        /// Sets specified values for display.
        /// </summary>
        public void Setup(string label, string value)
        {
            this.label.Text = label;
            valueLabel.Text = value;
        }
    }
}