using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.UI.Components
{
    public interface ILogoDisplay : IGraphicObject, IHasAlpha {
    
        /// <summary>
        /// Returns the glowing sprite on the logo.
        /// </summary>
        ISprite Glow { get; }

        /// <summary>
        /// Returns the outer border on the logo.
        /// </summary>
        ISprite Outer { get; }

        /// <summary>
        /// Returns the inner background on the logo.
        /// </summary>
        ISprite Inner { get; }

        /// <summary>
        /// Returns the title on the logo.
        /// </summary>
        ISprite Title { get; }
    }
}