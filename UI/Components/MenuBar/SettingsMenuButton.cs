using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class SettingsMenuButton : IconMenuButton {

        protected override string IconName => "icon-settings";


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
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