using PBGame.UI.Components.Common;
using PBGame.UI.Navigations.Overlays;
using PBGame.Data.Users;
using PBGame.Graphics;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class MenuHolder : UguiObject {

        private BoxButton detailButton;
        private BoxButton visitButton;
        private BoxButton logoutButton;
        private ILabel accountLabel;


        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


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

                detailButton.OnTriggered += () =>
                {
                    // Hide this overlay
                    OverlayNavigator.Hide<ProfileMenuOverlay>();
                    // TODO: Show profile screen.
                };
            }
            visitButton = CreateChild<BoxButton>("visit", 1);
            {
                visitButton.Anchor = AnchorType.MiddleStretch;
                visitButton.RawWidth = -96f;
                visitButton.Y = 10f;
                visitButton.Height = 36f;
                visitButton.LabelText = "Visit";
                visitButton.Color = colorPreset.Warning;

                visitButton.OnTriggered += () =>
                {
                    // Open browser to the user homepage.
                    if(UserManager.CurrentUser.Value != null)
                        Application.OpenURL(UserManager.CurrentUser.Value.OnlineUser.ProfilePage);
                };
            }
            logoutButton = CreateChild<BoxButton>("logout", 2);
            {
                logoutButton.Anchor = AnchorType.MiddleStretch;
                logoutButton.RawWidth = -96f;
                logoutButton.Y = -30f;
                logoutButton.Height = 36f;
                logoutButton.LabelText = "Log out";
                logoutButton.Color = colorPreset.Negative;

                logoutButton.OnTriggered += () =>
                {
                    DoLogout();
                };
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

            var user = UserManager.CurrentUser.Value;
            if(user != null && user.OnlineUser.Api != null)
                accountLabel.Text = $"Logged in using {user.OnlineUser.Api.ApiType}";
            else
                accountLabel.Text = $"You are currently offline.";
        }

        /// <summary>
        /// Handles logout process.
        /// </summary>
        private void DoLogout()
        {
            var dialog = OverlayNavigator.Show<DialogOverlay>();
            dialog.SetMessage("Would you like to log out?");
            dialog.AddConfirmCancel(() =>
            {
                var user = UserManager.CurrentUser.Value;
                if(user != null && user.OnlineUser.Api != null)
                    user.OnlineUser.Api.Logout();
                    
                UserManager.SaveUser(user);
                UserManager.RemoveUser();
            });
        }
    }
}