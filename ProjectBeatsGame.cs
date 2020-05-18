using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Rulesets.Maps;
using PBGame.Graphics;
using PBGame.Networking.API;
using PBGame.Notifications;
using PBFramework.Data;
using UnityEngine;

namespace PBGame
{
    public class ProjectBeatsGame : BaseGame
    {
        /// <summary>
        /// Holds the original screen size before modifying with game configurations.
        /// </summary>
        private Vector2 originalScreenSize;

        private Cached<MenuBarOverlay> cachedMenuBar = new Cached<MenuBarOverlay>();


        /// <summary>
        /// Returns whether the initial splash view should be shown automatically.
        /// </summary>
        public virtual bool ShouldShowFirstView => true;


        protected override void PostInitialize()
        {
            base.PostInitialize();

            originalScreenSize = new Vector2(Screen.width, Screen.height);

            // Hook events
            HookEngine();
            HookMusicController();
            HookScreenNavigator();
            HookOverlayNavigator();
            HookMapSelection();
            HookConfigurations();
            HookDownloadStore();
            HookMapManager();

            // Display splash view.
            if (ShouldShowFirstView)
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
                // Loop the music when not in game screen.
                if(!(screenNavigator.CurrentScreen is GameScreen))
                {
                    // TODO: This may have a bug where music won't loop in home screen when there's only one mapset.
                    // Check whether menu bar exists and try letting the music menu handle music switching.
                    var menuBar = overlayNavigator.Get<MenuBarOverlay>();
                    if (menuBar != null && menuBar.MusicButton.Active)
                    {
                        menuBar.MusicButton.SetNextMusic();
                    }
                    // Else if homescreen, select a random music.
                    else if (screenNavigator.CurrentScreen is HomeScreen)
                    {
                        mapSelection.SelectMapset(mapManager.AllMapsets.GetRandom());
                    }
                    // Else if download screen, just stop there.
                    else if (screenNavigator.CurrentScreen is DownloadScreen)
                    {
                        musicController.Stop();
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
                    if (view is HomeScreen || view is DownloadScreen || view is GameScreen)
                        musicController.LoopTime = 0f;
                    else
                        musicController.LoopTime = mapSelection.Map.Metadata.PreviewTime;
                }

                ApplyMenuBarOverlay();
                ApplyMenuBarProperties();
            };
            screenNavigator.OnScreenChange += (curScreen, prevScreen) =>
            {
                // If navigating out of game screen, play music if stopped.
                if (prevScreen is GameScreen)
                {
                    if (!musicController.IsPlaying)
                    {
                        bool wasPaused = musicController.IsPaused;
                        musicController.Play();
                        // Play from beginning if music stopped at the end.
                        if (!wasPaused)
                        {
                            musicController.Seek(0f);
                            musicController.Fade(0f, 1f);
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Triggers actions on cerain overlay navigator events.
        /// </summary>
        private void HookOverlayNavigator()
        {
            overlayNavigator.OnShowView += (view) =>
            {
                if(!(view is MenuBarOverlay))
                    ApplyMenuBarOverlay();
                else
                {
                    if (!cachedMenuBar.IsValid)
                    {
                        cachedMenuBar.Value = view as MenuBarOverlay;
                        ApplyMenuBarProperties();
                    }
                }
            };
            overlayNavigator.OnHideView += (view) =>
            {
                if (!(view is MenuBarOverlay))
                    ApplyMenuBarOverlay();
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
                    if (previewTime < 0)
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
                soundPool.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.EffectVolume.Value);
            };
            gameConfiguration.MusicVolume.OnValueChanged += (volume, _) =>
            {
                musicController.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.MusicVolume.Value);
            };
            gameConfiguration.EffectVolume.OnValueChanged += (volume, _) =>
            {
                soundPool.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.EffectVolume.Value);
            };

            // Mapset sort change events
            gameConfiguration.MapsetSort.OnValueChanged += (sort, _) =>
            {
                mapManager.Sort(sort);
            };

            // Parallax events
            gameConfiguration.UseParallax.OnValueChanged += (useParallax, _) =>
            {
                inputManager.UseAcceleration = useParallax;
            };

            // Resolution & framerate change events
            gameConfiguration.ResolutionQuality.OnValueChanged += (quality, _) =>
            {
                ApplyScreenResolution();
            };
            gameConfiguration.Framerate.OnValueChanged += (framerate, _) =>
            {
                ApplyScreenResolution();
            };

            // Offset settings
            gameConfiguration.GlobalOffset.OnValueChanged += (offset, _) =>
            {
                musicController.Clock.Offset = offset;
            };
        }

        /// <summary>
        /// Triggers actions on certain download store events.
        /// </summary>
        private void HookDownloadStore()
        {
            // Handle mapset import
            downloadStore.MapStorage.OnAdded += (path) =>
            {
                Debug.Log("ProjectBeatsGame.HookDownloadStore - Downloaded at path: " + path);
                mapManager.Import(downloadStore.MapStorage.GetFile(path));

                notificationBox.Add(new Notification()
                {
                    Message = $"Successfully downloaded mapset at ({path})",
                });
            };
        }

        /// <summary>
        /// Triggers action on certain map manager events.
        /// </summary>
        private void HookMapManager()
        {
            mapManager.OnImportMapset += (mapset) =>
            {
                if(mapset == null)
                    return;
                    
                // Select new map if in songs selection.
                if(screenNavigator.CurrentScreen is SongsScreen)
                    mapSelection.SelectMapset(mapset);

                notificationBox.Add(new Notification()
                {
                    Message = $"Imported mapset ({mapset.Metadata.Artist} - {mapset.Metadata.Title})",
                });
            };
        }

        /// <summary>
        /// Applies menu bar overlay where necessary.
        /// </summary>
        private void ApplyMenuBarOverlay()
        {
            if (screenNavigator.CurrentScreen is HomeScreen)
            {
                if(overlayNavigator.IsShowing(typeof(HomeMenuOverlay)))
                    overlayNavigator.Show<MenuBarOverlay>(true);
                else
                    overlayNavigator.Hide<MenuBarOverlay>();
                return;
            }

            if (overlayNavigator.IsShowing(typeof(GameLoadOverlay)) ||
                screenNavigator.CurrentScreen is GameScreen ||
                screenNavigator.CurrentScreen is InitializeScreen ||
                screenNavigator.CurrentScreen is SplashScreen)
            {
                overlayNavigator.Hide<MenuBarOverlay>();
                return;
            }

            // Show menu ber by default.
            overlayNavigator.Show<MenuBarOverlay>(true);
        }

        /// <summary>
        /// Applies menu bar properties on screen change.
        /// </summary>
        private void ApplyMenuBarProperties()
        {
            if (!cachedMenuBar.IsValid)
                return;

            var menuBar = cachedMenuBar.Value;
            bool isHomeActive = screenNavigator.CurrentScreen is HomeScreen;

            menuBar.MusicButton.Active = isHomeActive;
            menuBar.BackgroundSprite.Color = new Color(0f, 0f, 0f, 0f);
        }

        /// <summary>
        /// Applies screen resolution and framerate settings.
        /// </summary>
        private void ApplyScreenResolution()
        {
            Vector2 newResolution = originalScreenSize * gameConfiguration.ResolutionQuality.Value.GetResolutionScale();
            int framerate = gameConfiguration.Framerate.Value.GetFrameRate();
            Screen.SetResolution((int)newResolution.x, (int)newResolution.y, FullScreenMode.ExclusiveFullScreen, framerate);

            Application.targetFrameRate = framerate;
        }
    }
}