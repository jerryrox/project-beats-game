using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Networking.API;
using UnityEngine;

namespace PBGame
{
    public class ProjectBeatsGame : BaseGame {

        /// <summary>
        /// Returns whether the initial splash view should be shown automatically.
        /// </summary>
        public virtual bool ShouldShowFirstView => true;


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
            if(ShouldShowFirstView)
                screenNavigator.Show<SplashScreen>();
        }

        public override void GracefulQuit()
        {
            // Confirm quit
            var dialog = overlayNavigator.Show<DialogOverlay>();
            dialog.SetMessage("Are you sure you want to quit Project: Beats?");
            dialog.AddConfirmCancel(OnConfirmQuit, null);
        }

        public override void ForceQuit()
        {
            // Store configurations first
            gameConfiguration.Save();
            mapConfiguration.Save();
            mapsetConfiguration.Save();

            base.ForceQuit();
        }

        /// <summary>
        /// Event called from dialog when user decided to quit the game.
        /// </summary>
        private void OnConfirmQuit()
        {
            var quitView = overlayNavigator.Show<QuitOverlay>();
            quitView.OnQuitAniEnd += () =>
            {
                Debug.LogWarning("Quit");
                base.ForceQuit();
            };
        }

        /// <summary>
        /// Triggers actions on certain system events.
        /// </summary>
        private void HookEngine()
        {
            // TODO: These don't really help at early development yet. Come back when game is ready for play.

            // Start listening to any exceptions that occurs during game.
            // AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) =>
            // {
            //     var exception = e.ExceptionObject as Exception;
            //     Debug.LogError($"Unhandled exception: {exception.ToString()}");
            // };
            // Application.logMessageReceived += (condition, stackTrace, type) =>
            // {
            //     if (type == LogType.Exception)
            //     {
            //         Debug.LogError($"Unhandled exception at: {stackTrace}");
            //     }
            // };
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
                    // TODO: This may have a bug where music won't loop in home screen when there's only one mapset.
                    // Check whether menu bar exists and try letting the music menu handle music switching.
                    var menuBar = overlayNavigator.Get<MenuBarOverlay>();
                    if (menuBar.MusicButton.Active)
                    {
                        menuBar.MusicButton.SetNextMusic();
                    }
                    // Else if homescreen, select a random music.
                    else if (screenNavigator.CurrentScreen is HomeScreen)
                    {
                        mapSelection.SelectMapset(mapManager.AllMapsets.GetRandom());
                    }
                    else
                    {
                        musicController.Play();
                        musicController.Seek(musicController.LoopTime);
                        musicController.Fade(0f, 1f);
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