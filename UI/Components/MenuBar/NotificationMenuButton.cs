using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class NotificationMenuButton : BaseMenuButton
    {
        protected override string IconName => "icon-notification";


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            OnFocused += (isFocused) =>
            {
                if (isFocused)
                {
                    // TODO: Show notification overlay.
                    // TODO: Toggle off when notification overlay is hidden.
                }
                else
                {
                    // TODO: Hide notification overlay.
                }
            };
        }
    }
}