using PBGame.Rulesets.UI.HUD;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class HudContainer : Rulesets.UI.HudContainer {

        [InitWithDependency]
        private void Init()
        {
        }

        protected override IScoreDisplay CreateScoreDisplay()
        {
            var display = CreateChild<ScoreDisplay>("score-display");
            // TODO: Assign positions and size
            
            return display;
        }

        protected override IHealthDisplay CreateHealthDisplay()
        {
            var display = CreateChild<HealthDisplay>("health-display");
            // TODO: Assign positions and size
            return display;
        }

        protected override IAccuracyDisplay CreateAccuracyDisplay()
        {
            var display = CreateChild<AccuracyDisplay>("accuracy-display");
            // TODO: Assign positions and size
            return display;
        }
    }
}