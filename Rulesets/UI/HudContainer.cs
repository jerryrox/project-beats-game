using PBGame.Rulesets.UI.HUD;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.UI
{
    public abstract class HudContainer : UguiObject {

        /// <summary>
        /// Returns the accuracy displayer object.
        /// </summary>
        public IAccuracyDisplay AccuracyDisplay { get; private set; }

        /// <summary>
        /// Returns the health displayer object.
        /// </summary>
        public IHealthDisplay HealthDisplay { get; private set; }

        /// <summary>
        /// Returns the score displayer object.
        /// </summary>
        public IScoreDisplay ScoreDisplay { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            AccuracyDisplay = CreateAccuracyDisplay();
            {
                Depth = 0;
            }
            HealthDisplay = CreateHealthDisplay();
            {
                Depth = 2;
            }
            ScoreDisplay = CreateScoreDisplay();
            {
                Depth = 1;
            }
        }

        /// <summary>
        /// Creates a new accuracy displayer object.
        /// </summary>
        protected abstract IAccuracyDisplay CreateAccuracyDisplay();

        /// <summary>
        /// Creates a new helath displayer object.
        /// </summary>
        protected abstract IHealthDisplay CreateHealthDisplay();

        /// <summary>
        /// Creates a new score displayer object.
        /// </summary>
        protected abstract IScoreDisplay CreateScoreDisplay();
    }
}