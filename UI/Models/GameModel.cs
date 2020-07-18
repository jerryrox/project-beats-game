using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.Game;
using PBGame.UI.Navigations.Screens;
using PBGame.Maps;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;
using PBGame.Notifications;
using PBFramework;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class GameModel : BaseModel
    {
        private List<IExplicitPromise> gameLoaders = new List<IExplicitPromise>();
        private MultiPromise gameLoader;

        private IPlayableMap currentMap;
        private IModeService currentModeService;
        private IGameSession currentSession;

        private Bindable<GameLoadState> loadState = new Bindable<GameLoadState>();


        /// <summary>
        /// Returns whether game loading state is success.
        /// </summary>
        public bool IsLoaded => loadState.Value == GameLoadState.Success;

        /// <summary>
        /// Returns the current game loading state.
        /// </summary>
        public IReadOnlyBindable<GameLoadState> LoadState => loadState;

        /// <summary>
        /// Returns the game screen.
        /// </summary>
        private GameScreen Screen => ScreenNavigator.Get<GameScreen>();

        [ReceivesDependency]
        private INotificationBox NotificationBox { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

        [ReceivesDependency]
        private IRecordManager RecordManager { get; set; }


        /// <summary>
        /// Adds the specified promise as one of the game loaders.
        /// </summary>
        public void AddAsLoader(IExplicitPromise promise)
        {
            if (promise != null)
                gameLoaders.Add(promise);
        }

        /// <summary>
        /// Starts loading the game for the specified map and mode service.
        /// </summary>
        public void LoadGame(IPlayableMap map, IModeService modeService)
        {
            if(loadState.Value != GameLoadState.Idle)
                return;
            if(!ValidateLoadParams(map, modeService))
                return;

            loadState.Value = GameLoadState.Loading;

            currentMap = map;
            currentModeService = modeService;

            InitSession();
        }

        /// <summary>
        /// Cancels current game loading process.
        /// </summary>
        public void CancelLoad()
        {
            DisposeLoader();
            DisposeSession();
        }

        /// <summary>
        /// Starts the actual game session.
        /// </summary>
        public void StartGame()
        {
            currentSession.InvokeSoftInit();
        }

        /// <summary>
        /// Makes the user exit the game with a clear result.
        /// </summary>
        // TODO: Navigate to ResultScreen.
        public void ExitGameWithClear() => ExitTo<PrepareScreen>();

        /// <summary>
        /// Makes the user exit the game back to preparation screen.
        /// </summary>
        public void ExitGameForceful() => ExitTo<PrepareScreen>();

        /// <summary>
        /// Records the specified play record under the current player.
        /// </summary>
        public IExplicitPromise RecordScore(IScoreProcessor scoreProcessor, int playTime)
        {
            if(scoreProcessor == null || scoreProcessor.JudgeCount <= 0)
                return new ProxyPromise();

            // Retrieve user and user stats.
            var user = UserManager.CurrentUser.Value;
            if(user == null)
                return new ProxyPromise();
            var userStats = user.GetStatistics(currentMap.PlayableMode);
            if (userStats == null)
                return new ProxyPromise();

            // Record the play result to records database and user statistics.
            Record newRecord = new Record(currentMap, user, scoreProcessor, playTime);
            return new ProxyPromise(resolve =>
            {
                var recordProgress = new ReturnableProgress<IEnumerable<IRecord>>();
                recordProgress.OnFinished += (records) =>
                {
                    if (scoreProcessor.IsFinished)
                    {
                        RecordManager.SaveRecord(newRecord);

                        var bestRecord = RecordManager.GetBestRecord(records);
                        userStats.RecordPlay(newRecord, bestRecord);
                    }
                    else
                    {
                        userStats.RecordIncompletePlay(newRecord);
                    }
                };
                RecordManager.GetRecords(currentMap, recordProgress);
            });
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            loadState.Value = GameLoadState.Idle;
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            loadState.Value = GameLoadState.Idle;

            DisposeSession();
            DisposeLoader();

            currentMap = null;
            currentModeService = null;
        }

        /// <summary>
        /// Makes the user exit to the specified screen.
        /// </summary>
        private void ExitTo<T>()
            where T : MonoBehaviour, INavigationView
        {
            var screen = ScreenNavigator.Show<T>();
            // TODO: If Result screen, pass the newRecord object to ResultScreen.
            // if(screen is ResultScreen)
        }

        /// <summary>
        /// Initializes a new game session and starts loading the game.
        /// </summary>
        private void InitSession()
        {
            if(currentSession != null)
                throw new InvalidOperationException("Attempted to initialize a redundant game session.");

            currentSession = currentModeService.GetSession(Screen, Dependency);
            currentSession.SetMap(currentMap);
            currentSession.InvokeHardInit();
        }

        /// <summary>
        /// Disposes current game session.
        /// </summary>
        private void DisposeSession()
        {
            if(currentSession == null)
                return;
            currentSession.InvokeHardDispose();
            currentSession = null;
        }

        /// <summary>
        /// Initializes the game loader processes.
        /// </summary>
        private void InitLoader()
        {
            if(gameLoader != null)
                throw new InvalidOperationException("Attempted to initialize a redundant game loader process.");

            gameLoader = new MultiPromise(gameLoaders);
            gameLoader.OnFinished += () =>
            {
                loadState.Value = GameLoadState.Success;
            };
            gameLoader.Start();
        }

        /// <summary>
        /// Disposes all loading processes.
        /// </summary>
        private void DisposeLoader()
        {
            // Cancel all game loaders.
            gameLoaders.ForEach(p => p.Revoke());
            gameLoaders.Clear();
            // Dispose game loader
            if (gameLoader != null)
            {
                gameLoader.Revoke();
                gameLoader = null;
            }
        }

        /// <summary>
        /// Checks whether the game load parameters are valid and returns whether validation is a success.
        /// </summary>
        private bool ValidateLoadParams(IPlayableMap map, IModeService modeService)
        {
            // If invalid parameters, loading must fail.
            if (map == null)
            {
                NotificationBox?.Add(new Notification()
                {
                    Message = "Map is not specified!",
                    Type = NotificationType.Negative
                });
                loadState.Value = GameLoadState.Fail;
                return false;
            }
            if (modeService == null)
            {
                NotificationBox?.Add(new Notification()
                {
                    Message = "Game mode is not specified!",
                    Type = NotificationType.Negative
                });
                loadState.Value = GameLoadState.Fail;
                return false;
            }
            return true;
        }
    }
}