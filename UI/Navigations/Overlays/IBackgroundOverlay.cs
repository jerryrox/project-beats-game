using PBGame.UI.Components.Background;
using PBFramework.UI.Navigations;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IBackgroundOverlay : INavigationView {
        
        /// <summary>
        /// Returns the background displayer which displays the raw image.
        /// </summary>
        IBackgroundDisplay ImageBackground { get; }

        /// <summary>
        /// Returns the background displayer which displays the abstract gradient color based on the map background.
        /// </summary>
        IBackgroundDisplay GradientBackground { get; }
    }
}