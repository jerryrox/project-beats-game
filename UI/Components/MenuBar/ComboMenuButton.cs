using PBGame.UI.Models.MenuBar;

namespace PBGame.UI.Components.MenuBar
{
    public class ComboMenuButton : BaseMenuButton {

        protected override MenuType Type => MenuType.Quick;

        protected override string IconSpritename => "icon-menu";
    }
}