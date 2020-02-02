using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class ComboMenuButton : IconMenuButton {

        protected override string IconName => "icon-menu";


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            OnToggleOn += () =>
            {
                // TODO: Display combo menu
                // TODO: Toggle off the button on combo menu hidden event.
            };
            OnToggleOff += () =>
            {
                // TODO: Disable combo menu
            };
        }
    }
}