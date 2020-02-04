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
            HookScreenNavigator();
            HookMapSelection();
            HookConfigurations();

            // Display splash view.
            screenNavigator.Show<SplashScreen>();
        }

        // TODO: Implement music controller looping.
        // private void 

        /// <summary>
        /// Triggers actions on certain screen navigator events.
        /// </summary>
        private void HookScreenNavigator()
        {
            screenNavigator.OnShowView += (view) =>
            {
                if (mapSelection.Map != null)
                {
                    // Change loop time based on the screens.
                    // TODO: Uncomment when game screen is implemented.
                    if (view is HomeScreen)// || view is GameScreen)
                        musicController.LoopTime = 0f;
                    else
                        musicController.LoopTime = mapSelection.Map.Metadata.PreviewTime;
                }
            };
        }

        /// <summary>
        /// Triggers actions on certain map selection events.
        /// </summary>
        private void HookMapSelection()
        {
            mapSelection.OnMusicLoaded += (music) =>
            {
                // Play music on load.
                musicController.MountAudio(music);
                musicController.Play();
                // Seek to preview time if not home screen.
                if (!(screenNavigator.CurrentScreen is HomeScreen))
                    musicController.Seek(mapSelection.Map.Metadata.PreviewTime);
            };
            mapSelection.OnMusicUnloaded += () =>
            {
                // Stop and unload music.
                musicController.Stop();
                musicController.MountAudio(null);
            };
        }

        /// <summary>
        /// Triggers actions on certain configuration events.
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