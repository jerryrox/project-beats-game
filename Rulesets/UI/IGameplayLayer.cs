using PBFramework.Graphics;

namespace PBGame.Rulesets.UI
{
    public interface IGameplayLayer : IGraphicObject {
    
        /// <summary>
        /// Returns the play area container object.
        /// </summary>
        IPlayAreaContainer PlayArea { get; }

        /// <summary>
        /// Returns the hud container object.
        /// </summary>
        IHudContainer Hud { get; }
    }
}