using PBGame.UI.Components.MenuBar;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IMenuBarOverlay : INavigationView {
    
        /// <summary>
        /// Returns the background sprite of the menu bar.
        /// </summary>
        IBackgroundSprite BackgroundSprite { get; }

        /// <summary>
        /// Returns the combo menu button.
        /// </summary>
        IMenuButton ComboMenuButton { get; }

        /// <summary>
        /// Returns the profile info button.
        /// </summary>
        IMenuButton ProfileButton { get; }

        /// <summary>
        /// Returns the settings menu button.
        /// </summary>
        IMenuButton SettingsMenuButton { get; }

        /// <summary>
        /// Returns the notification menu button.
        /// </summary>
        IMenuButton NotificationMenuButton { get; }
    }
}