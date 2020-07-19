using PBGame.UI.Models.MenuBar;

namespace PBGame.UI.Components.MenuBar
{
    public class NotificationMenuButton : BaseMenuButton
    {
        protected override MenuType Type => MenuType.Notification;

        protected override string IconSpritename => "icon-notification";
    }
}