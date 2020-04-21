using PBGame.UI.Components.MenuBar;
using PBGame.Configurations.Settings;
using PBFramework.UI.Navigations;

namespace PBGame.UI.Navigations.Overlays
{
    public interface ISettingsMenuOverlay : INavigationView, ISubMenuOverlay
    {
        /// <summary>
        /// Builds settings menu for the specified settings data.
        /// </summary>
        void SetSettingsData(ISettingsData data);
    }
}