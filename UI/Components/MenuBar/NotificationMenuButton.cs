using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class NotificationMenuButton : BaseMenuButton
    {
        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            IconName = "icon-notification";

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