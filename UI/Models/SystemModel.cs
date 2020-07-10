using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
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

        private BindableBool isMenuBarActive = new BindableBool();
        private BindableBool isGameScreen = new BindableBool();


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

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        protected override void OnPreShow()
        {
            base.OnPreShow();

            isMenuBarActive.Value = OverlayNavigator.IsActive(typeof(MenuBarOverlay));
            isGameScreen.Value = ScreenNavigator.CurrentScreen.Value is GameScreen;

            OverlayNavigator.OnShowView += OnOverlayShow;
            OverlayNavigator.OnHideView += OnOverlayHide;

            ScreenNavigator.OnShowView += OnScreenShow;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            OverlayNavigator.OnShowView -= OnOverlayShow;
            OverlayNavigator.OnHideView -= OnOverlayHide;

            ScreenNavigator.OnShowView -= OnScreenShow;
        }

        /// <summary>
        /// Event called when the specified overlay has been shown.
        /// </summary>
        private void OnOverlayShow(INavigationView view)
        {
            if(view is MenuBarOverlay)
                isMenuBarActive.Value = true;
        }

        /// <summary>
        /// Event called when the specified overlay has been hidden.
        /// </summary>
        private void OnOverlayHide(INavigationView view)
        {
            if(view is MenuBarOverlay)
                isMenuBarActive.Value = false;
        }

        /// <summary>
        /// Event called when the specified screen has been shown.
        /// </summary>
        private void OnScreenShow(INavigationView view)
        {
            isGameScreen.Value = view is GameScreen;
        }
    }
}