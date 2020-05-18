using PBGame.Rulesets.UI.HUD;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.UI
{
    public abstract class HudContainer : UguiObject
    {
        /// <summary>
        /// Returns the accuracy displayer object.
        /// </summary>
        public AccuracyDisplay AccuracyDisplay { get; protected set; }

        /// <summary>
        /// Returns the score displayer object.
        /// </summary>
        public ScoreDisplay ScoreDisplay { get; protected set; }

        /// <summary>
        /// Returns the combo displayer object.
        /// </summary>
        public ComboDisplay ComboDisplay { get; protected set; }

        /// <summary>
        /// Returns the health displayer object.
        /// </summary>
        public HealthDisplay HealthDisplay { get; protected set; }
    }
}