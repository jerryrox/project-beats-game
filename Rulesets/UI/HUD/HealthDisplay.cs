using PBGame.Rulesets.Scoring;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.UI.HUD
{
    public class HealthDisplay : UguiObject {

        private bool isFailing;
        private IScoreProcessor scoreProcessor;


        /// <summary>
        /// The health progress bar.
        /// </summary>
        public IProgressBar ProgressBar { get; private set; }

        /// <summary>
        /// The sprite which indicates the required amount of health to clear the map.
        /// </summary>
        public ISprite Indicator { get; private set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init(IGameSession gameSession)
        {
            gameSession.OnSoftInit += () =>
            {
                scoreProcessor = gameSession.ScoreProcessor;
                scoreProcessor.Health.BindAndTrigger(OnHealthChange);
                SetFailing(false);
            };
            gameSession.OnSoftDispose += () =>
            {
                ProgressBar.Value = 0f;
            };

            ProgressBar = CreateChild<UguiProgressBar>("progress", 0);
            {
                ProgressBar.Foreground.Color = ColorPreset.PrimaryFocus.Base;
            }
            Indicator = CreateChild<UguiSprite>("indicator", 1);
            {
                Indicator.Color = ColorPreset.PrimaryFocus.Base;
            }
        }

        /// <summary>
        /// Visually changes the progress bar to indicate whether the player is failing or not.
        /// </summary>
        private void SetFailing(bool isFailing)
        {
            if(this.isFailing == isFailing)
                return;

            this.isFailing = isFailing;
            ProgressBar.Foreground.Color = isFailing ? ColorPreset.Negative.Base : ColorPreset.PrimaryFocus.Base;
        }

        /// <summary>
        /// Event called on health change.
        /// </summary>
        private void OnHealthChange(float health, float prevHealth)
        {
            ProgressBar.Value = health;
            SetFailing(scoreProcessor.IsFailed);
        }
    }
}