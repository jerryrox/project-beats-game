using PBGame.UI.Components.Home;
using PBFramework.UI;

namespace PBGame.UI.Navigations.Screens
{
    public interface IHomeScreen : IBaseScreen {
    
        /// <summary>
        /// Returns the main logo on the screen.
        /// </summary>
        ILogoDisplay LogoDisplay { get; }

        /// <summary>
        /// Returns the sprite for blurring the background on focus.
        /// </summary>
        ISprite FocusBlur { get; }
    }
}