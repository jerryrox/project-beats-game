using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;

namespace PBGame
{
    public class ProjectBeatsGame : BaseGame {

        protected override void PostInitialize()
        {
            base.PostInitialize();
            
            // Hook events
            HookMusicController();
            HookConfigurations();

            // Display splash view.
            screenNavigator.Show<SplashScreen>();
        }

        /// <summary>
        /// Triggers actions on music controller on certain events from other dependencies.
        /// </summary>
        private void HookMusicController()
        {
        }

        /// <summary>
        /// Listens to certain configuration change events and apply changes.
        /// </summary>
        private void HookConfigurations()
        {
            gameConfiguration.MasterVolume.OnValueChanged += (volume, _) =>
            {
                musicController.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.MusicVolume.Value);
                soundPooler.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.EffectVolume.Value);
            };
            gameConfiguration.MusicVolume.OnValueChanged += (volume, _) =>
            {
                musicController.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.MusicVolume.Value);
            };
            gameConfiguration.EffectVolume.OnValueChanged += (volume, _) =>
            {
                soundPooler.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.EffectVolume.Value);
            };
        }
    }
}