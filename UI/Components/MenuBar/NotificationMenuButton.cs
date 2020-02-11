using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class NotificationMenuButton : IconMenuButton
    {
        protected override string IconName => "icon-notification";


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            OnToggleOn += () =>
            {
                // TODO: Show notification overlay.
                // TODO: Toggle off when notification overlay is hidden.
            };

            OnToggleOff += () =>
            {
                // TODO: Hide notification overlay.
            };
        }
    }
}