using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class HomeMenuModel : BaseModel {

        /// <summary>
        /// Returns the background of the map currently selected.
        /// </summary>
        public IReadOnlyBindable<IMapBackground> Background => MapSelection.Background;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IGame Game { get; set; }


        /// <summary>
        /// Attempts to navigate to songs selection screen for play.
        /// </summary>
        public void PlayGame()
        {
            ScreenNavigator.Show<SongsScreen>();
            HideMenu();
        }

        /// <summary>
        /// Attempts to navigate to download screen.
        /// </summary>
        public void DownloadMaps()
        {
            ScreenNavigator.Show<DownloadScreen>();
            HideMenu();
        }

        /// <summary>
        /// Attempts to quit the game.
        /// </summary>
        public void QuitGame() => Game.GracefulQuit();

        /// <summary>
        /// Attempts to hide the home menu overlay.
        /// </summary>
        public void HideMenu() => OverlayNavigator.Hide<HomeMenuOverlay>();
    }
}