using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Audio;
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

        /// <summary>
        /// The offset of the current mapset.
        /// </summary>
        private IMusicOffset mapsetOffset;

        /// <summary>
        /// The offset of the current map.
        /// </summary>
        private IMusicOffset mapOffset;

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
            HookApi();

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
                if (!(screenNavigator.CurrentScreen.Value is GameScreen))
                {
                    // TODO: This may have a bug where music won't loop in home screen when there's only one mapset.
                    // Check whether menu bar exists and try letting the music menu handle music switching.
                    var menuBar = overlayNavigator.Get<MenuBarOverlay>();
                    if (menuBar != null && menuBar.MusicButton.Active)
                    {
                        menuBar.MusicButton.SetNextMusic();
                    }
                    // Else if homescreen, select a random music.
                    else if (screenNavigator.CurrentScreen.Value is HomeScreen)
                    {
                        mapSelection.SelectMapset(mapManager.AllMapsets.GetRandom());
                    }
                    // Else if download screen, just stop there.
                    else if (screenNavigator.CurrentScreen.Value is DownloadScreen)
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
                if (mapSelection.Map.Value != null)
                {
                    // Change loop time based on the screens.
                    if (view is HomeScreen || view is DownloadScreen || view is GameScreen)
                        musicController.LoopTime = 0f;
                    else
                        musicController.LoopTime = mapSelection.Map.Value.Metadata.PreviewTime;
                }

                ApplyMenuBarOverlay();
                ApplyMenuBarProperties();
            };
            screenNavigator.CurrentScreen.OnValueChanged += (curScreen, prevScreen) =>
            {
                // If navigating out of game screen, play music if stopped.
                if (prevScreen is GameScreen)
                {
                    if (!musicController.IsPlaying)
                    {
                        bool wasPaused = musicController.IsPaused;
                        musicController.Play();
                        // Play from preview point if music stopped at the end.
                        if (!wasPaused)
                        {
                            musicController.Seek(mapSelection.Map.Value.Metadata.PreviewTime);
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
                if(view is OffsetsOverlay)
                    musicController.SetVolume(0.5f);

                if (!(view is MenuBarOverlay))
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
                if(view is OffsetsOverlay)
                    musicController.SetVolume(1f);

                if (!(view is MenuBarOverlay))
                    ApplyMenuBarOverlay();
            };
        }

        /// <summary>
        /// Triggers actions on certain map selection events.
        /// </summary>
        private void HookMapSelection()
        {
            mapSelection.MapConfig.OnValueChanged += (config, prevConfig) =>
            {
                // Observe offset changes in the map.
                if(prevConfig != null)
                    prevConfig.Offset.OnValueChanged -= OnMusicOffsetChange;
                mapOffset = config;
                if(mapOffset != null)
                    mapOffset.Offset.BindAndTrigger(OnMusicOffsetChange);
            };
            mapSelection.MapsetConfig.OnValueChanged += (config, prevConfig) =>
            {
                // Observe offset changes in the mapset.
                if(prevConfig != null)
                    prevConfig.Offset.OnValueChanged -= OnMusicOffsetChange;
                mapsetOffset = config;
                if(mapsetOffset != null)
                    mapsetOffset.Offset.BindAndTrigger(OnMusicOffsetChange);
            };

            mapSelection.Map.OnNewValue += (map) =>
            {
                metronome.CurrentMap = map;
            };

            mapSelection.Music.OnNewValue += (music) =>
            {
                musicController.MountAudio(music);
                if (music == null)
                {
                    musicController.Stop();
                    return;
                }

                musicController.Play();

                // Seek to preview time if not home screen.
                if (!(screenNavigator.CurrentScreen.Value is HomeScreen))
                {
                    var previewTime = mapSelection.Map.Value.Metadata.PreviewTime;
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
        }

        /// <summary>
        /// Triggers actions on certain configuration events.
        /// </summary>
        private void HookConfigurations()
        {
            // Game volume change events
            gameConfiguration.MasterVolume.OnNewValue += (volume) =>
            {
                ApplyMusicVolume();
                soundPool.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.EffectVolume.Value);
            };
            gameConfiguration.MusicVolume.OnNewValue += (volume) =>
            {
                ApplyMusicVolume();
            };
            gameConfiguration.EffectVolume.OnNewValue += (volume) =>
            {
                soundPool.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.EffectVolume.Value);
            };

            // Mapset sort change events
            gameConfiguration.MapsetSort.OnNewValue += (sort) =>
            {
                mapManager.Sort(sort);
            };

            // Parallax events
            gameConfiguration.UseParallax.OnNewValue += (useParallax) =>
            {
                inputManager.UseAcceleration = useParallax;
            };

            // Resolution & framerate change events
            gameConfiguration.ResolutionQuality.OnNewValue += (quality) =>
            {
                ApplyScreenResolution();
            };
            gameConfiguration.Framerate.OnNewValue += (framerate) =>
            {
                ApplyScreenResolution();
            };

            // Offset settings
            gameConfiguration.GlobalOffset.OnNewValue += (offset) =>
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
                if (mapset == null)
                    return;

                // Select new map if in songs selection.
                if (screenNavigator.CurrentScreen.Value is SongsScreen)
                    mapSelection.SelectMapset(mapset);

                notificationBox.Add(new Notification()
                {
                    Message = $"Imported mapset ({mapset.Metadata.Artist} - {mapset.Metadata.Title})",
                });
            };
        }

        /// <summary>
        /// Triggers action on certain API events.
        /// </summary>
        private void HookApi()
        {
            api.User.OnValueChanged += (user, oldUser) =>
            {
                // If user turned online
                if (user.IsOnline && user != oldUser)
                    OnUserBecameOnline(user);
                // If user turned offline
                else if (!user.IsOnline && user != oldUser)
                    OnUserBecameOffline(user);
            };
        }

        /// <summary>
        /// Applies menu bar overlay where necessary.
        /// </summary>
        private void ApplyMenuBarOverlay()
        {
            if (screenNavigator.CurrentScreen.Value is HomeScreen)
            {
                if (overlayNavigator.IsShowing(typeof(HomeMenuOverlay)))
                    overlayNavigator.Show<MenuBarOverlay>(true);
                else
                    overlayNavigator.Hide<MenuBarOverlay>();
                return;
            }

            if (overlayNavigator.IsShowing(typeof(GameLoadOverlay)) ||
                screenNavigator.CurrentScreen.Value is GameScreen ||
                screenNavigator.CurrentScreen.Value is InitializeScreen ||
                screenNavigator.CurrentScreen.Value is SplashScreen)
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
            bool isHomeActive = screenNavigator.CurrentScreen.Value is HomeScreen;

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

        /// <summary>
        /// Applies music offset to music controller.
        /// </summary>
        private void ApplyMusicOffset() => musicController.Clock.Offset = GetMusicOffset();

        /// <summary>
        /// Applies volume to music controller.
        /// </summary>
        private void ApplyMusicVolume(float scale = 1f)
        {
            musicController.SetVolume(gameConfiguration.MasterVolume.Value * gameConfiguration.MusicVolume.Value * scale);
        }

        /// <summary>
        /// Event called when the user in API has become online.
        /// </summary>
        private void OnUserBecameOnline(IOnlineUser user)
        {
            userManager.SetUser(user, null);
        }

        /// <summary>
        /// Event called when the user in API has become offline.
        /// </summary>
        private void OnUserBecameOffline(IOnlineUser user)
        {
            userManager.RemoveUser();
        }

        /// <summary>
        /// Event called when the mapset/map's offset has been changed.
        /// </summary>
        private void OnMusicOffsetChange(int offset, int prevOffset) => ApplyMusicOffset();

        /// <summary>
        /// Returns the total music offset applied.
        /// </summary>
        private float GetMusicOffset()
        {
            return gameConfiguration.GlobalOffset.Value +
                (mapOffset != null ? mapOffset.Offset.Value : 0) +
                (mapsetOffset != null ? mapsetOffset.Offset.Value : 0);
        }
    }
}