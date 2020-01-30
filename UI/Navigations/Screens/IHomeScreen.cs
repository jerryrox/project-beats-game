using PBGame.UI.Components.Home;

namespace PBGame.UI.Navigations.Screens
{
    public interface IHomeScreen : IBaseScreen {
    
        /// <summary>
        /// Returns the main logo on the screen.
        /// </summary>
        ILogoDisplay LogoDisplay { get; }

        
    }
}