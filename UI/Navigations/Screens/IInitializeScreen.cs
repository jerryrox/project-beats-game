using PBGame.UI.Components.Initialize;

namespace PBGame.UI.Navigations.Screens
{
    public interface IInitializeScreen : IBaseScreen {
    
        /// <summary>
        /// Returns the logo displayer on the screen.
        /// </summary>
        ILogoDisplay LogoDisplay { get; }

        /// <summary>
        /// Returns the load displayer on the screen.
        /// </summary>
        ILoadDisplay LoadDisplay { get; }
    }
}