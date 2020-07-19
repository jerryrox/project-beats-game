using PBGame.UI.Models.MenuBar;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class MusicButton : BaseMenuButton {

        protected override MenuType Type => MenuType.Music;

        protected override string IconSpritename => "icon-music";
    }
}