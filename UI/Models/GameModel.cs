using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.UI.Models.Game;
using PBGame.UI.Navigations.Screens;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;
using PBGame.Notifications;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.UI.Models
{
    public class GameModel : BaseModel
    {
        private List<ITask> gameLoaders = new List<ITask>();
        private MultiTask gameLoader;

        private IPlayableMap currentMap;
        private IModeService currentModeService;
        private IGameSession currentSession;
        private IRecord lastRecord;

        private Bindable<GameLoadState> loadState = new Bindable<GameLoadState>(GameLoadState.Idle);


        /// <summary>
        /// Returns whether game loading state is success.
        /// </summary>
        public bool IsLoaded => loadState.Value == GameLoadState.Success;

        /// <summary>
        /// Returns the current game session.
        /// </summary>
        public IGameSession CurrentSession => currentSession;

        /// <summary>
        /// Returns the current game loading state.
        /// </summary>
        public IReadOnlyBindable<GameLoadState> LoadState => loadState;

        /// <summary>
        /// Returns the current mode servicer instance.
        /// </summary>
        public IModeService ModeService => currentModeService;

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
        /// Adds the specified task as one of the game loaders.
        /// </summary>
        public void AddAsLoader(ITask task)
        {
            if (task != null)
                gameLoaders.Add(task);
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
            InitLoader();
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
        public void ExitGameWithClear() => ExitTo<ResultScreen>();

        /// <summary>
        /// Makes the user exit the game back to preparation screen.
        /// </summary>
        public void ExitGameForceful() => ExitTo<PrepareScreen>();

        /// <summary>
        /// Records the specified play record under the current player.
        /// </summary>
        public Task RecordScore(IScoreProcessor scoreProcessor, int playTime, TaskListener listener = null)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (scoreProcessor == null || scoreProcessor.JudgeCount <= 0)
                    {
                        listener?.SetFinished();
                        return;
                    }

                    // Retrieve user and user stats.
                    var user = UserManager.CurrentUser.Value;
                    var userStats = user.GetStatistics(currentMap.PlayableMode);

                    // Record the play result to records database and user statistics.
                    Record newRecord = new Record(currentMap, user, scoreProcessor, playTime);
                    lastRecord = newRecord;
                    var records = await RecordManager.GetRecords(currentMap, user, listener?.CreateSubListener<List<IRecord>>());

                    // Save as cleared play.
                    if (scoreProcessor.IsFinished)
                    {
                        RecordManager.SaveRecord(newRecord);

                        var bestRecord = RecordManager.GetBestRecord(records);
                        userStats.RecordPlay(newRecord, bestRecord);
                    }
                    // Save as failed play.
                    else
                    {
                        userStats.RecordIncompletePlay(newRecord);
                    }
                    listener?.SetFinished();
                }
                catch (Exception e)
                {
                    Logger.LogError($"Error while recording score: {e.Message}\n{e.StackTrace}");
                    listener?.SetFinished();
                }
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
            lastRecord = null;
        }

        /// <summary>
        /// Makes the user exit to the specified screen.
        /// </summary>
        private void ExitTo<T>()
            where T : MonoBehaviour, INavigationView
        {
            var record = lastRecord;
            var screen = ScreenNavigator.Show<T>();
            if (screen is ResultScreen resultScreen)
                resultScreen.Model.Setup(currentMap, record);
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

            gameLoader = new MultiTask(gameLoaders);
            gameLoader.OnFinished += () =>
            {
                loadState.Value = GameLoadState.Success;
            };
            gameLoader.StartTask();
        }

        /// <summary>
        /// Disposes all loading processes.
        /// </summary>
        private void DisposeLoader()
        {
            // Cancel all game loaders.
            gameLoaders.ForEach(p => p.RevokeTask(true));
            gameLoaders.Clear();
            // Dispose game loader
            if (gameLoader != null)
            {
                gameLoader.RevokeTask(true);
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