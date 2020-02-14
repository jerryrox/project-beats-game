using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Graphics;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class MenuHolder : UguiObject, IMenuHolder {

        private IMenuButton detailButton;
        private IMenuButton visitButton;
        private IMenuButton logoutButton;
        private ILabel accountLabel;


        /// <summary>
        /// Returns the osu api from the manager.
        /// </summary>
        private IApi OsuApi => ApiManager.GetApi(ApiProviders.Osu);

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset, IScreenNavigator screenNavigator)
        {
            detailButton = CreateChild<MenuButton>("detail", 0);
            {
                detailButton.Anchor = Anchors.MiddleStretch;
                detailButton.RawWidth = -96f;
                detailButton.Y = 50f;
                detailButton.Height = 36f;
                detailButton.LabelText = "Detail";
                detailButton.Tint = colorPreset.Positive;

                detailButton.OnPointerClick += () =>
                {
                    // Hide this overlay
                    OverlayNavigator.Hide<ProfileMenuOverlay>();
                    // TODO: Show profile screen.
                };
            }
            visitButton = CreateChild<MenuButton>("visit", 1);
            {
                visitButton.Anchor = Anchors.MiddleStretch;
                visitButton.RawWidth = -96f;
                visitButton.Y = 10f;
                visitButton.Height = 36f;
                visitButton.LabelText = "Visit";
                visitButton.Tint = colorPreset.Warning;

                visitButton.OnPointerClick += () =>
                {
                    // Open browser to the user homepage.
                    Application.OpenURL(OsuApi.User.Value.ProfilePage);
                };
            }
            logoutButton = CreateChild<MenuButton>("logout", 2);
            {
                logoutButton.Anchor = Anchors.MiddleStretch;
                logoutButton.RawWidth = -96f;
                logoutButton.Y = -30f;
                logoutButton.Height = 36f;
                logoutButton.LabelText = "Log out";
                logoutButton.Tint = colorPreset.Negative;

                logoutButton.OnPointerClick += () =>
                {
                    DoLogout();
                };
            }
            accountLabel = CreateChild<Label>("account", 3);
            {
                accountLabel.Anchor = Anchors.MiddleStretch;
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

            accountLabel.Text = $"Logged in using {OsuApi.ApiType}";
        }

        /// <summary>
        /// Handles logout process.
        /// </summary>
        private void DoLogout()
        {
            var dialog = OverlayNavigator.Show<DialogOverlay>();
            dialog.SetMessage("Would you like to log out?");
            dialog.AddConfirmCancel(() => OsuApi.Logout());
        }
    }
}