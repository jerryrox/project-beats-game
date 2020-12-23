using PBGame.UI.Models.MenuBar;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Data.Users;
using PBGame.Rulesets;
using PBGame.Networking.API;
using PBGame.Configurations;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Models
{
    public class MenuBarModel : BaseModel {

        private Bindable<MenuType> focusedMenu = new Bindable<MenuType>(MenuType.None);
        private Bindable<INavigationView> currentOverlay = new Bindable<INavigationView>();
        private BindableBool isMusicButtonActive = new BindableBool(false);
        private Bindable<Color> barColor = new Bindable<Color>();


        /// <summary>
        /// Returns the menu currently focused.
        /// </summary>
        public IReadOnlyBindable<MenuType> FocusedMenu => focusedMenu;

        /// <summary>
        /// Returns the menu overlay currently displayed.
        /// </summary>
        public IReadOnlyBindable<INavigationView> CurrentOverlay => currentOverlay;

        /// <summary>
        /// Returns whether the music button should be activated.
        /// </summary>
        public IReadOnlyBindable<bool> IsMusicButtonActive => isMusicButtonActive;

        /// <summary>
        /// Returns the menu bar background color.
        /// </summary>
        public IReadOnlyBindable<Color> BarColor => barColor;

        /// <summary>
        /// Returns the current user loaded.
        /// </summary>
        public IReadOnlyBindable<IUser> CurrentUser => UserManager.CurrentUser;

        /// <summary>
        /// Returns the current game mode.
        /// </summary>
        public IReadOnlyBindable<GameModeType> GameMode => GameConfiguration.RulesetMode;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

        [ReceivesDependency]
        private IApi Api { get; set; }


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

        /// <summary>
        /// Returns the mode service instance for the current game mode.
        /// </summary>
        public IModeService GetModeService()
        {
            return ModeManager.GetService(GameMode.Value);
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            ScreenNavigator.CurrentScreen.BindAndTrigger(OnScreenChange);

            SetMenu(MenuType.None);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            ScreenNavigator.CurrentScreen.OnNewValue -= OnScreenChange;

            HideMenu();
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
        /// Returns the appropriate menu overlay for the specified type.
        /// </summary>
        private INavigationView GetMenuFor(MenuType type)
        {
            switch (type)
            {
                case MenuType.Music: return OverlayNavigator.Show<MusicMenuOverlay>();
                case MenuType.Notification: return OverlayNavigator.Show<NotificationMenuOverlay>();
                case MenuType.Profile: return OverlayNavigator.Show<ProfileMenuOverlay>();
                case MenuType.Mode: return OverlayNavigator.Show<ModeMenuOverlay>();
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
        /// Event called when the current screen changes.
        /// </summary>
        private void OnScreenChange(INavigationView screen)
        {
            isMusicButtonActive.Value = (screen is HomeScreen);
            barColor.Value = new Color();
        }
    }
}