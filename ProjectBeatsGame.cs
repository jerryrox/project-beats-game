using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;

namespace PBGame
{
    public class ProjectBeatsGame : BaseGame {

        protected override void PostInitialize()
        {
            // Hook events
            HookMusicController();

            // Display splash view.
            screenNavigator.Show<SplashScreen>();
        }

        /// <summary>
        /// Triggers actions on music controller on certain events from other dependencies.
        /// </summary>
        private void HookMusicController()
        {
        }
    }
}