using PBGame.UI.Navigations.Overlays;
using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class ComboMenuButton : BaseMenuButton {

        private bool hasOverlay = false;


        protected override string IconSpritename => "icon-menu";


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            OnFocused += (isFocused) =>
            {
                if (isFocused)
                {
                    var overlay = overlayNavigator.Show<QuickMenuOverlay>();
                    overlay.OnClose += () =>
                    {
                        hasOverlay = false;
                        IsFocused = false;
                    };
                    hasOverlay = true;
                }
                else
                {
                    if (hasOverlay)
                        overlayNavigator.Hide<QuickMenuOverlay>();
                    hasOverlay = false;
                }
            };
        }
    }
}