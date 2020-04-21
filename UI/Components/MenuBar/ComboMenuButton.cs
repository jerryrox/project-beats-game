using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class ComboMenuButton : BaseMenuButton {

        protected override string IconSpritename => "icon-menu";


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            OnFocused += (isFocused) =>
            {
                if (isFocused)
                {
                    // TODO: Display combo menu
                    // TODO: Toggle off the button on combo menu hidden event.
                }
                else
                {
                    // TODO: Disable combo menu
                }
            };
        }
    }
}