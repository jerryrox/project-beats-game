using PBGame.UI.Components.MenuBar;
using PBFramework.UI.Navigations;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IMusicMenuOverlay : INavigationView, ISubMenuOverlay {
    
        /// <summary>
        /// The music button which triggered this overlay.
        /// </summary>
        MusicButton MusicButton { get; set; }
    }
}