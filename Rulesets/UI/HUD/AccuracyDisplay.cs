using PBGame.UI;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI.HUD
{
    public class AccuracyDisplay : UguiObject
    {
        /// <summary>
        /// The label displaying the accuracy.
        /// </summary>
        public ILabel Label { get; private set; }


        [InitWithDependency]
        private void Init(IGameSession gameSession)
        {
            gameSession.OnSoftInit += () =>
            {
                gameSession.ScoreProcessor.Accuracy.BindAndTrigger(OnAccuracyChange);
            };
            gameSession.OnSoftDispose += () =>
            {
                Label.Text = "0%";
            };

            this.Size = Vector2.zero;

            Label = CreateChild<Label>("label");
            {
                Label.Size = Vector2.zero;
            }
        }

        /// <summary>
        /// Event called when the accuracy changes.
        /// </summary>
        private void OnAccuracyChange(float acc, float prevAcc)
        {
            Label.Text = acc.ToString("P2");
        }
    }
}