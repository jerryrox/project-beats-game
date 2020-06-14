using PBGame.UI.Components.MenuBar;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IMenuBarOverlay : INavigationView {

        /// <summary>
        /// Returns the height of the container.
        /// </summary>
        float ContainerHeight { get; }

        /// <summary>
        /// Returns the background sprite of the menu bar.
        /// </summary>
        BackgroundSprite BackgroundSprite { get; }

        /// <summary>
        /// Returns the combo menu button.
        /// </summary>
        BaseMenuButton ComboMenuButton { get; }

        /// <summary>
        /// Returns the profile info button.
        /// </summary>
        BaseMenuButton ProfileButton { get; }

        /// <summary>
        /// Returns the music player button.
        /// </summary>
        MusicButton MusicButton { get; }

        /// <summary>
        /// Returns the settings menu button.
        /// </summary>
        BaseMenuButton SettingsMenuButton { get; }

        /// <summary>
        /// Returns the notification menu button.
        /// </summary>
        BaseMenuButton NotificationMenuButton { get; }
    }
}