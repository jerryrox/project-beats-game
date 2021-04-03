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
using PBFramework.Threading;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

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

        /// <summary>
        /// Log service for piping log messages to the notification box.
        /// </summary>
        private LogToNotificationService logToNotifService;


        /// <summary>
        /// Returns whether the initial splash view should be shown automatically.
        /// </summary>
        public virtual bool ShouldShowFirstView => true;


        protected override void PostInitialize()
        {
            base.PostInitialize();

            originalScreenSize = new Vector2(Screen.width, Screen.height);
            // Initialize log service for notification.
            logToNotifService = new LogToNotificationService()
            {
                NotificationBox = base.notificationBox,
            };
            Logger.Register(logToNotifService);

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
            dialog.Model.SetMessage("Are you sure you want to quit Project: Beats?");
            dialog.Model.AddConfirmCancel(OnConfirmQuit, null);
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
            overlayNavigator.Show<QuitOverlay>();
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
                Logger.LogError($"Unhandled exception: {exception.ToString()}");
            };
            Application.logMessageReceived += (condition, stackTrace, type) =>
            {
                if (type == LogType.Exception)
                {
                    Logger.LogError($"Unhandled exception: {condition}\n{stackTrace}");
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
                // Loop the music when not in game screen.
                if (!(screenNavigator.CurrentScreen.Value is GameScreen))
                {
                    if (screenNavigator.CurrentScreen.Value is HomeScreen)
                    {
                        mapSelection.SelectMapset(musicPlaylist.PeekNext());
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
                if (prevConfig != null)
                    prevConfig.Offset.OnValueChanged -= OnMusicOffsetChange;
                mapsetOffset = config;
                if (mapsetOffset != null)
                    mapsetOffset.Offset.BindAndTrigger(OnMusicOffsetChange);
            };

            mapSelection.Map.OnNewValue += (map) =>
            {
                metronome.CurrentMap = map;
            };
            mapSelection.Mapset.OnNewValue += (mapset) =>
            {
                musicPlaylist.Focus(mapset);
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
            // Action request events
            gameConfiguration.OnRequestGameRepo += () =>
            {
                Application.OpenURL(App.GameRepository);
            };
            gameConfiguration.OnRequestFrameworkRepo += () =>
            {
                Application.OpenURL(App.FrameworkRepository);
            };
            gameConfiguration.OnRequestMapsetReload += () =>
            {
                // TODO:
            };
            gameConfiguration.OnRequestMapsetCheck += () =>
            {
                LoadMapsetsInDownloads();
            };

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

            // Offset events
            gameConfiguration.GlobalOffset.OnNewValue += (offset) =>
            {
                musicController.Clock.Offset = offset;
            };

            // Game mode events
            gameConfiguration.RulesetMode.OnNewValue += (mode) =>
            {
                NotifyUnplayableMode();
            };

            // Notification related
            gameConfiguration.PersistNotificationLevel.OnNewValue += (level) =>
            {
                notificationBox.ForceStoreLevel = level;
            };
            gameConfiguration.LogToNotificationLevel.OnNewValue += (level) =>
            {
                logToNotifService.PipeLogLevel = level;
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
            mapManager.AllMapsets.OnChange += (mapsets) =>
            {
                musicPlaylist.Refill(mapsets);
            };
        }

        /// <summary>
        /// Triggers action on certain API events.
        /// </summary>
        private void HookApi()
        {
            api.User.OnValueChanged += (user, oldUser) =>
            {
                OnApiUserChange(user);
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
        /// Applies screen resolution and framerate settings.
        /// </summary>
        private void ApplyScreenResolution()
        {
            Vector2 newResolution = originalScreenSize * gameConfiguration.ResolutionQuality.Value.GetResolutionScale();
            int framerate = gameConfiguration.Framerate.Value.GetFrameRate();
            Screen.SetResolution((int)newResolution.x, (int)newResolution.y, FullScreenMode.ExclusiveFullScreen, framerate);

            Application.targetFrameRate = framerate;

            inputManager.SetResolution(newResolution);
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
        /// Shows a notification to the user if the current game mode is unplayable yet.
        /// </summary>
        private void NotifyUnplayableMode()
        {
            var gameMode = gameConfiguration.RulesetMode.Value;
            var modeService = modeManager.GetService(gameMode);
            if (modeService != null)
            {
                if (!modeService.IsPlayable)
                {
                    notificationBox.Add(new Notification()
                    {
                        Message = $"The selected game mode ({modeService.Name}) is not playable yet. Any maps selected will be playable by its original, or the first available mode.",
                        Scope = NotificationScope.Temporary,
                        Type = NotificationType.Warning,
                    });
                }
            }
        }

        /// <summary>
        /// Loads all mapsets in the downloads folder.
        /// </summary>
        private void LoadMapsetsInDownloads()
        {
            TaskListener listener = new TaskListener();
            string id = new Guid().ToString();
            listener.OnFinished += () =>
            {
                notificationBox.RemoveById(id, false);
            };

            foreach (var archive in downloadStore.MapStorage.GetAllFiles())
                mapManager.Import(archive, listener.CreateSubListener<IMapset>());

            if (listener.SubListenerCount == 0)
            {
                notificationBox.Add(new Notification()
                {
                    Message = "There are no mapsets to load.",
                });
            }
            else
            {
                notificationBox.Add(new Notification()
                {
                    Listener = listener,
                    Message = $"Loading {listener.SubListenerCount} mapsets in downloads directory.",
                    Type = NotificationType.Info,
                    Id = id,
                });
            }
        }

        /// <summary>
        /// Returns the total music offset applied.
        /// </summary>
        private float GetMusicOffset()
        {
            return gameConfiguration.GlobalOffset.Value +
                (mapOffset != null ? mapOffset.Offset.Value : 0) +
                (mapsetOffset != null ? mapsetOffset.Offset.Value : 0);
        }

        /// <summary>
        /// Event called when the user in API has become online.
        /// </summary>
        private void OnApiUserChange(IOnlineUser user)
        {
            userManager.SaveUser(userManager.CurrentUser.Value);
            userManager.SetUser(user);
        }

        /// <summary>
        /// Event called when the mapset/map's offset has been changed.
        /// </summary>
        private void OnMusicOffsetChange(int offset, int prevOffset)
        {
            ApplyMusicOffset();
        }
    }
}