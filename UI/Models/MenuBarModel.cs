using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.MenuBar;
using PBGame.UI.Navigations.Overlays;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class MenuBarModel : BaseModel {

        private Bindable<MenuType> focusedMenu = new Bindable<MenuType>(MenuType.None);

        private Bindable<INavigationView> currentOverlay = new Bindable<INavigationView>();


        /// <summary>
        /// Returns the menu currently focused.
        /// </summary>
        public IReadOnlyBindable<MenuType> FocusedMenu => focusedMenu;

        /// <summary>
        /// Returns the menu overlay currently displayed.
        /// </summary>
        public IReadOnlyBindable<INavigationView> CurrentOverlay => currentOverlay;

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


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

            SetMenu(MenuType.None);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

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
    }
}