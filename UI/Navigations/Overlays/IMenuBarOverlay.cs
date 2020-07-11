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
    }
}