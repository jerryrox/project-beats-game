using PBFramework.UI.Navigations;

namespace PBGame.UI.Navigations.Screens
{
    public interface IPrepareScreen : INavigationView {

        /// <summary>
        /// Sets whether info container's detailed section should be shown.
        /// </summary>
        void ToggleInfoDetail();
    }
}