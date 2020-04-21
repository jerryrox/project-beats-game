using PBGame.UI.Navigations.Overlays;
using PBGame.Configurations;
using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class SettingsMenuButton : BaseMenuButton {

        private bool hasOverlay = false;


        protected override string IconSpritename => "icon-settings";


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator, IGameConfiguration gameConfiguration)
        {
            OnFocused += (isFocused) =>
            {
                if (isFocused)
                {
                    var overlay = overlayNavigator.Show<SettingsMenuOverlay>();
                    overlay.SetSettingsData(gameConfiguration.Settings);
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
                        overlayNavigator.Hide<SettingsMenuOverlay>();
                    hasOverlay = false;
                }
            };
        }
    }
}