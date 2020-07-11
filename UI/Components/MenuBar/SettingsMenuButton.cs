using PBGame.UI.Models.MenuBar;

namespace PBGame.UI.Components.MenuBar
{
    public class SettingsMenuButton : BaseMenuButton {

        protected override MenuType Type => MenuType.Settings;

        protected override string IconSpritename => "icon-settings";
    }
}