using PBGame.UI.Models.MenuBar;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Data.Users;
using PBGame.Assets.Caching;
using PBGame.Networking.API;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Allocation.Caching;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Models
{
    public class MenuBarModel : BaseModel {

        private CacherAgent<Texture2D> profileImageCacher;

        private Bindable<MenuType> focusedMenu = new Bindable<MenuType>(MenuType.None);
        private Bindable<INavigationView> currentOverlay = new Bindable<INavigationView>();
        private Bindable<Texture2D> profileImage = new Bindable<Texture2D>();
        private BindableBool isMusicButtonActive = new BindableBool(false);


        /// <summary>
        /// Returns the menu currently focused.
        /// </summary>
        public IReadOnlyBindable<MenuType> FocusedMenu => focusedMenu;

        /// <summary>
        /// Returns the menu overlay currently displayed.
        /// </summary>
        public IReadOnlyBindable<INavigationView> CurrentOverlay => currentOverlay;

        /// <summary>
        /// Returns the profile image of the current online user.
        /// </summary>
        public IReadOnlyBindable<Texture2D> ProfileImage => profileImage;

        /// <summary>
        /// Returns whether the music button should be activated.
        /// </summary>
        public IReadOnlyBindable<bool> IsMusicButtonActive => isMusicButtonActive;

        /// <summary>
        /// Returns the current user loaded.
        /// </summary>
        public IReadOnlyBindable<IUser> CurrentUser => UserManager.CurrentUser;

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

        [ReceivesDependency]
        private IApi Api { get; set; }

        [ReceivesDependency]
        private IWebImageCacher WebImageCacher { get; set; }


        [InitWithDependency]
        private void Init()
        {
            profileImageCacher = new CacherAgent<Texture2D>(WebImageCacher);
            profileImageCacher.OnFinished += OnProfileImageLoaded;
        }

        /// <summary>
        /// Sets the current menu type.
        /// </summary>
        public void SetMenu(MenuType type)
        {
            // If same type, unfocus everything.
            if (type == focusedMenu.Value)
            {
                focusedMenu.Value = MenuType.None;
                HideMenu();
            }
            else
            {
                focusedMenu.Value = type;
                ShowMenu(type);
            }
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            CurrentUser.BindAndTrigger(OnUserChange);
            ScreenNavigator.CurrentScreen.BindAndTrigger(OnScreenChange);

            SetMenu(MenuType.None);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            CurrentUser.OnNewValue -= OnUserChange;
            ScreenNavigator.CurrentScreen.OnNewValue -= OnScreenChange;

            HideMenu();
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            RemoveProfileImage();
        }

        /// <summary>
        /// Shows the menu overlay for specified type.
        /// </summary>
        private void ShowMenu(MenuType type)
        {
            HideMenu();

            var menu = GetMenuFor(type);
            if(menu == null)
                return;

            currentOverlay.Value = menu;
            menu.OnHide += OnOverlayHide;
        }

        /// <summary>
        /// Hides the current menu overlay.
        /// </summary>
        private void HideMenu()
        {
            var menu = currentOverlay.Value;
            if(menu == null)
                return;

            ReleaseMenu();
            OverlayNavigator.Hide(menu);
        }

        /// <summary>
        /// Release reference to the last opened menu overlay.
        /// </summary>
        private void ReleaseMenu()
        {
            var menu = currentOverlay.Value;
            if(menu == null)
                return;

            menu.OnHide -= OnOverlayHide;
            currentOverlay.Value = null;
        }

        /// <summary>
        /// Removes current profile image.
        /// </summary>
        private void RemoveProfileImage()
        {
            profileImage.Value = null;
            profileImageCacher.Remove();
        }

        /// <summary>
        /// Returns the appropriate menu overlay for the specified type.
        /// </summary>
        private INavigationView GetMenuFor(MenuType type)
        {
            switch (type)
            {
                case MenuType.Music: return OverlayNavigator.Show<MusicMenuOverlay>();
                case MenuType.Notification: return null;//OverlayNavigator.Show<NotificationMenuOverlay>();
                case MenuType.Profile: return OverlayNavigator.Show<ProfileMenuOverlay>();
                case MenuType.Quick: return OverlayNavigator.Show<QuickMenuOverlay>();
                case MenuType.Settings: return OverlayNavigator.Show<SettingsMenuOverlay>();
            }
            return null;
        }

        /// <summary>
        /// Event called when the current menu overlay is hiding.
        /// </summary>
        private void OnOverlayHide()
        {
            focusedMenu.Value = MenuType.None;
            ReleaseMenu();
        }

        /// <summary>
        /// Event called when the web image cacher has returned a new proile image.
        /// </summary>
        private void OnProfileImageLoaded(Texture2D image)
        {
            profileImage.Value = image;
        }

        /// <summary>
        /// Event called on user profile change.
        /// </summary>
        private void OnUserChange(IUser user)
        {
            RemoveProfileImage();
            if(user != null && !string.IsNullOrEmpty(user.OnlineUser.AvatarImage))
                profileImageCacher.Request(user.OnlineUser.AvatarImage);
        }

        /// <summary>
        /// Event called when the current screen changes.
        /// </summary>
        private void OnScreenChange(INavigationView screen)
        {
            isMusicButtonActive.Value = (screen is HomeScreen);
        }
    }
}