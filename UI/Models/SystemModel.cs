using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Notifications;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class SystemModel : BaseModel {

        /// <summary>
        /// Event called on new notification from the notification box.
        /// </summary>
        public event Action<INotification> OnNewNotification;

        private BindableBool isMenuBarActive = new BindableBool();
        private BindableBool isGameScreen = new BindableBool();
        private BindableBool isNotificationOverlayActive = new BindableBool();


        /// <summary>
        /// Returns whether fps display should be enabled.
        /// </summary>
        public IReadOnlyBindable<bool> IsFpsEnabled => GameConfiguration.ShowFps;

        /// <summary>
        /// Returns whether message display should be enabled.
        /// </summary>
        public IReadOnlyBindable<bool> IsMessageEnabled => GameConfiguration.DisplayMessages;

        /// <summary>
        /// Retuns whether message display should be enabled during game.
        /// </summary>
        public IReadOnlyBindable<bool> IsMessageEnabledGame => GameConfiguration.DisplayMessagesInGame;

        /// <summary>
        /// Returns whether the menu bar is currently active.
        /// </summary>
        public IReadOnlyBindable<bool> IsMenuBarActive => isMenuBarActive;

        /// <summary>
        /// Returns whether the game screen is currently active.
        /// </summary>
        public IReadOnlyBindable<bool> IsGameScreen => isGameScreen;

        /// <summary>
        /// Returns whether the notification menu overlay is currently active.
        /// </summary>
        public IReadOnlyBindable<bool> IsNotificationOverlayActive => isNotificationOverlayActive;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private INotificationBox NotificationBox { get; set; }


        /// <summary>
        /// Removes the specified notification from the notification box.
        /// </summary>
        public void RemoveNotification(INotification notification)
        {
            if(notification.Scope == NotificationScope.Temporary)
                NotificationBox.Remove(notification);
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            isMenuBarActive.Value = OverlayNavigator.IsActive(typeof(MenuBarOverlay));
            isGameScreen.Value = ScreenNavigator.CurrentScreen.Value is GameScreen;

            OverlayNavigator.OnShowView += OnOverlayShow;
            OverlayNavigator.OnHideView += OnOverlayHide;

            ScreenNavigator.OnShowView += OnScreenShow;

            NotificationBox.OnNewNotification += OnNotification;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            OverlayNavigator.OnShowView -= OnOverlayShow;
            OverlayNavigator.OnHideView -= OnOverlayHide;

            ScreenNavigator.OnShowView -= OnScreenShow;

            NotificationBox.OnNewNotification -= OnNotification;
        }

        /// <summary>
        /// Event called when the specified overlay has been shown.
        /// </summary>
        private void OnOverlayShow(INavigationView view)
        {
            if(view is MenuBarOverlay)
                isMenuBarActive.Value = true;
            if(view is NotificationMenuOverlay)
                isNotificationOverlayActive.Value = true;
        }

        /// <summary>
        /// Event called when the specified overlay has been hidden.
        /// </summary>
        private void OnOverlayHide(INavigationView view)
        {
            if(view is MenuBarOverlay)
                isMenuBarActive.Value = false;
            if(view is NotificationMenuOverlay)
                isNotificationOverlayActive.Value = false;
        }

        /// <summary>
        /// Event called when the specified screen has been shown.
        /// </summary>
        private void OnScreenShow(INavigationView view)
        {
            isGameScreen.Value = view is GameScreen;
        }

        /// <summary>
        /// Event called when a new notification arrives.
        /// </summary>
        private void OnNotification(INotification notification)
        {
            OnNewNotification?.Invoke(notification);
        }
    }
}