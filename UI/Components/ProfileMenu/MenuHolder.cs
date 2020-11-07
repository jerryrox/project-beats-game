using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Data.Users;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.ProfileMenu
{
    public class MenuHolder : UguiObject {

        private BoxButton detailButton;
        private BoxButton visitButton;
        private BoxButton logoutButton;
        private ILabel accountLabel;


        [ReceivesDependency]
        private ProfileMenuModel Model { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset, IScreenNavigator screenNavigator)
        {
            detailButton = CreateChild<BoxButton>("detail", 0);
            {
                detailButton.Anchor = AnchorType.MiddleStretch;
                detailButton.RawWidth = -96f;
                detailButton.Y = 50f;
                detailButton.Height = 36f;
                detailButton.LabelText = "Detail";
                detailButton.Color = colorPreset.Positive;

                detailButton.OnTriggered += Model.ShowUserDetail;
            }
            visitButton = CreateChild<BoxButton>("visit", 1);
            {
                visitButton.Anchor = AnchorType.MiddleStretch;
                visitButton.RawWidth = -96f;
                visitButton.Y = 10f;
                visitButton.Height = 36f;
                visitButton.LabelText = "Visit";
                visitButton.Color = colorPreset.Warning;

                visitButton.OnTriggered += Model.VisitUserPage;
            }
            logoutButton = CreateChild<BoxButton>("logout", 2);
            {
                logoutButton.Anchor = AnchorType.MiddleStretch;
                logoutButton.RawWidth = -96f;
                logoutButton.Y = -30f;
                logoutButton.Height = 36f;
                logoutButton.LabelText = "Log out";
                logoutButton.Color = colorPreset.Negative;

                logoutButton.OnTriggered += Model.LogoutUser;
            }
            accountLabel = CreateChild<Label>("account", 3);
            {
                accountLabel.Anchor = AnchorType.MiddleStretch;
                accountLabel.RawWidth = 0f;
                accountLabel.Y = -81f;
                accountLabel.Height = 30;
                accountLabel.WrapText = true;
                accountLabel.FontSize = 16;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.CurrentUser.BindAndTrigger(OnUserChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.CurrentUser.OnNewValue -= OnUserChange;
        }

        /// <summary>
        /// Event called on local user change.
        /// </summary>
        private void OnUserChange(IUser user)
        {
            if(user.IsOnlineUser)
                accountLabel.Text = $"Logged in using {user.OnlineUser.Provider.Name}";
            else
                accountLabel.Text = $"You are currently offline.";
        }
    }
}