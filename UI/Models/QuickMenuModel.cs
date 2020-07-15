using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.QuickMenu;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class QuickMenuModel : BaseModel {

        private MenuInfo[] menus;


        [ReceivesDependency]
        private IGame Game { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            menus = new MenuInfo[]
            {
                new MenuInfo()
                {
                    Action = CreateMenuAction(NavigateToScreen<HomeScreen>),
                    HighlightCondition = IsScreenActive<HomeScreen>,
                    Icon = "icon-home",
                    Label = "Home"
                },
                new MenuInfo()
                {
                    Action = CreateMenuAction(NavigateToScreen<SongsScreen>),
                    HighlightCondition = IsScreenActive<SongsScreen>,
                    Icon = "icon-play",
                    Label = "Play"
                },
                new MenuInfo()
                {
                    Action = CreateMenuAction(NavigateToScreen<PrepareScreen>),
                    HighlightCondition = IsScreenActive<PrepareScreen>,
                    Icon = "icon-game",
                    Label = "Prepare"
                },
                new MenuInfo()
                {
                    Action = CreateMenuAction(NavigateToScreen<DownloadScreen>),
                    HighlightCondition = IsScreenActive<DownloadScreen>,
                    Icon = "icon-download",
                    Label = "Download"
                },
                new MenuInfo()
                {
                    Action = CreateMenuAction(Game.GracefulQuit),
                    Icon = "icon-power",
                    Label = "Quit"
                },
            };
        }

        /// <summary>
        /// Returns all menus to be displayed.
        /// </summary>
        public IEnumerable<MenuInfo> GetMenus() => menus;

        /// <summary>
        /// Closes the quick menu overlay.
        /// </summary>
        private void CloseQuickMenu() => OverlayNavigator.Hide<QuickMenuOverlay>();

        /// <summary>
        /// Creates a menu action which executes the specified action.
        /// </summary>
        private Action CreateMenuAction(Action action)
        {
            return () =>
            {
                action?.Invoke();
                CloseQuickMenu();
            };
        }

        /// <summary>
        /// Navigates to the specified screen.
        /// </summary>
        private void NavigateToScreen<T>()
            where T : MonoBehaviour, INavigationView
        {
            ScreenNavigator.Show<T>();
        }

        /// <summary>
        /// Returns whether the specified screen type is active.
        /// </summary>
        private bool IsScreenActive<T>() => ScreenNavigator.IsActive(typeof(T));
    }
}