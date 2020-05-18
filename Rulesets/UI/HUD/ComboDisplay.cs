using PBGame.UI;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI.HUD
{
    public class ComboDisplay : UguiObject
    {
        /// <summary>
        /// The label displaying the combo.
        /// </summary>
        public ILabel Label { get; private set; }


        [InitWithDependency]
        private void Init(IGameSession gameSession)
        {
            gameSession.OnSoftInit += () =>
            {
                gameSession.ScoreProcessor.Combo.BindAndTrigger(OnComboChange);
            };

            this.Size = Vector2.zero;

            Label = CreateChild<Label>("label");
            {
                Label.Size = Vector2.zero;
            }
        }

        /// <summary>
        /// Event called when the combo changes.
        /// </summary>
        private void OnComboChange(int combo, int prevCombo)
        {
            Label.Text = $"x{combo.ToString("N0")}";
        }
    }
}