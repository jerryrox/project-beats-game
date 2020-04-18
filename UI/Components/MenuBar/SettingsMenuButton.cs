using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class SettingsMenuButton : BaseMenuButton {

        protected override string IconName => "icon-settings";


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            OnFocused += (isFocused) =>
            {
                if (isFocused)
                {
                    // TODO: Show settings overlay.
                    // TODO: Toggle off when settings overlay is hidden.
                }
                else
                {
                    // TODO: Hide settings overlay.
                }
            };
        }
    }
}