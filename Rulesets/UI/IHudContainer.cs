using PBGame.Rulesets.UI.HUD;
using PBFramework.Graphics;

namespace PBGame.Rulesets.UI
{
    public interface IHudContainer : IGraphicObject {
    
        /// <summary>
        /// Returns the accuracy displayer object.
        /// </summary>
        IAccuracyDisplay AccuracyDisplay { get; }

        /// <summary>
        /// Returns the health displayer object.
        /// </summary>
        IHealthDisplay HealthDisplay { get; }

        /// <summary>
        /// Returns the score displayer object.
        /// </summary>
        IScoreDisplay ScoreDisplay { get; }
    }
}