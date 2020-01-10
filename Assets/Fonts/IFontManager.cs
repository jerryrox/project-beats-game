using PBFramework.Assets.Fonts;

namespace PBGame.Assets.Fonts
{
    public interface IFontManager {
    
        /// <summary>
        /// Returns the default font of the game.
        /// </summary>
        IFont DefaultFont { get; }
    }
}