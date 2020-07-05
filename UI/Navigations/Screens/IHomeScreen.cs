using PBGame.UI.Components.Home;

namespace PBGame.UI.Navigations.Screens
{
    public interface IHomeScreen {
    
        /// <summary>
        /// Returns the main logo on the screen.
        /// </summary>
        LogoDisplay LogoDisplay { get; }
    }
}