// TODO:
using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.Rulesets.UI.HUD
{
    public interface IAccuracyDisplay : IGraphicObject {
    
        /// <summary>
        /// Returns the label displaying the accuracy.
        /// </summary>
        ILabel AccuracyLabel { get; }
    }
}