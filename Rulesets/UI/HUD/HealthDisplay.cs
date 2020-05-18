using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.UI.HUD
{
    public class HealthDisplay : UguiObject {

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
                gameSession.ScoreProcessor.Health.BindAndTrigger(OnHealthChange);
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
        /// Event called on health change.
        /// </summary>
        private void OnHealthChange(float health, float prevHealth)
        {
            ProgressBar.Value = health;
        }
    }
}