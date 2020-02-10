using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using UnityEngine;

namespace PBGame
{
    public class ProjectBeatsGame : BaseGame {

        protected override void PostInitialize()
        {
            base.PostInitialize();

            // Hook events
            HookEngine();
            HookMusicController();
            HookScreenNavigator();
            HookMapSelection();
            HookConfigurations();

            // Display splash view.
            screenNavigator.Show<SplashScreen>();
        }

        /// <summary>
        /// Triggers actions on certain system events.
        /// </summary>
        private void HookEngine()
        {
            // Start listening to any exceptions that occurs during game.
            AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) =>
            {
                var exception = e.ExceptionObject as Exception;
                Debug.LogError($"Unhandled exception: {exception.ToString()}");
            };
            Application.logMessageReceived += (condition, stackTrace, type) =>
            {
                if (type == LogType.Exception)
                {
                    Debug.LogError($"Unhandled exception at: {stackTrace}");
                }
            };
        }

        /// <summary>
        /// Triggers actions on certain music controller events.
        /// </summary>
        private void HookMusicController()
        {
            musicController.OnEnd += () =>
            {
                // TODO: Loop the music when not in game screen.
                // if(!(screenNavigator.CurrentScreen is GameScreen))
                {
                    // Check whether menu bar exists and try letting the music menu handle music switching.
                    var menuBar = overlayNavigator.Get<MenuBarOverlay>();
                    // if ()
                    {
                    }
                    // else
                    {
                        musicController.Seek(musicController.LoopTime);
                        musicController.Play();
                    }
                }
                // Else, don't do anything.
            };
        }

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
                {
                    var previewTime = mapSelection.Map.Metadata.PreviewTime;
                    // Some songs don't have a proper preview time.
                    if(previewTime < 0)
                        previewTime = music.Duration / 2;

                    musicController.LoopTime = previewTime;
                    musicController.Seek(previewTime);
                }
                else
                {
                    musicController.LoopTime = 0f;
                }

                // Play song
                musicController.Fade(0f, 1f);
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
            // Game volume change events
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

            // Mapset sort change events
            gameConfiguration.MapsetSort.OnValueChanged += (sort, _) => 
            {
                mapManager.Sort(sort);
            };
        }
    }
}