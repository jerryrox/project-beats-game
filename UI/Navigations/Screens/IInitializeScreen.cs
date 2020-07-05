using PBGame.UI.Components.Initialize;

namespace PBGame.UI.Navigations.Screens
{
    public interface IInitializeScreen {
    
        /// <summary>
        /// Returns the logo displayer on the screen.
        /// </summary>
        LogoDisplay LogoDisplay { get; }

        /// <summary>
        /// Returns the load displayer on the screen.
        /// </summary>
        LoadDisplay LoadDisplay { get; }
    }
}