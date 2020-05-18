using PBGame.UI;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI.HUD
{
    public class ScoreDisplay : UguiObject
    {
        /// <summary>
        /// The label displaying the score.
        /// </summary>
        public ILabel Label { get; private set; }


        [InitWithDependency]
        private void Init(IGameSession gameSession)
        {
            gameSession.OnSoftInit += () =>
            {
                gameSession.ScoreProcessor.Score.BindAndTrigger(OnScoreChange);
            };

            this.Size = Vector2.zero;

            Label = CreateChild<Label>("label");
            {
                Label.Size = Vector2.zero;
            }
        }

        /// <summary>
        /// Event called when the score changes.
        /// </summary>
        private void OnScoreChange(int score, int prevScore)
        {
            Label.Text = score.ToString("N0");
        }
    }
}