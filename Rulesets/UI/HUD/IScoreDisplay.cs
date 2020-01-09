using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.Rulesets.UI.HUD
{
    public interface IScoreDisplay : IGraphicObject {
    
        /// <summary>
        /// Returns the label displaying the score.
        /// </summary>
        ILabel ScoreLabel { get; }
    }
}