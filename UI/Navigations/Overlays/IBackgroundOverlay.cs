using PBGame.UI.Components.Background;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IBackgroundOverlay : INavigationView, IHasColor {
        
        /// <summary>
        /// Returns the background displayer for an empty view.
        /// </summary>
        IBackgroundDisplay EmptyBackground { get; }

        /// <summary>
        /// Returns the background displayer which displays the raw image.
        /// </summary>
        IBackgroundDisplay ImageBackground { get; }

        /// <summary>
        /// Returns the background displayer which displays the abstract gradient color based on the map background.
        /// </summary>
        IBackgroundDisplay GradientBackground { get; }


        /// <summary>
        /// Sets the background displayer to focus.
        /// </summary>
        void SetBackground(IBackgroundDisplay display);
    }
}