using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class SettingsMenuButton : BaseMenuButton {

        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            IconName = "icon-settings";

            OnToggleOn += () =>
            {
                // TODO: Show settings overlay.
                // TODO: Toggle off when settings overlay is hidden.
            };

            OnToggleOff += () =>
            {
                // TODO: Hide settings overlay.
            };
        }
    }
}