using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.UI.Models.Game;
using PBGame.UI.Navigations.Screens;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Stores;
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

        private GameParameter currentParameter;
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
        /// Returns the record last made.
        /// </summary>
        public IRecord LastRecord => lastRecord;

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
        private IRecordStore RecordStore { get; set; }


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
        public void LoadGame(GameParameter parameter, IModeService modeService)
        {
            if(loadState.Value != GameLoadState.Idle)
                return;
            if(!ValidateLoadParams(parameter.Map, modeService))
                return;

            loadState.Value = GameLoadState.Loading;

            currentParameter = parameter;
            currentModeService = modeService;

            InitSession(parameter);
            CleanUpResources();
            
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
        public void ExitGameToResult()
        {
            var record = currentParameter.Record ?? lastRecord;
            var screen = ScreenNavigator.Show<ResultScreen>();
            screen.Model.Setup(currentParameter.Map, record);
        }

        /// <summary>
        /// Makes the user exit the game back to preparation screen.
        /// </summary>
        public void ExitGameForceful() => ScreenNavigator.Show<PrepareScreen>();

        /// <summary>
        /// Records the specified play record under the current player.
        /// </summary>
        public Task<IRecord> RecordScore(IScoreProcessor scoreProcessor, int playTime, TaskListener<IRecord> listener = null)
        {
            return Task.Run<IRecord>(async () =>
            {
                try
                {
                    if (scoreProcessor == null || scoreProcessor.JudgeCount <= 0)
                    {
                        listener?.SetFinished();
                        return null;
                    }

                    // Retrieve user and user stats.
                    var currentMap = currentParameter.Map;
                    var user = UserManager.CurrentUser.Value;
                    var userStats = user.GetStatistics(currentMap.PlayableMode);

                    // Record the play result to records database and user statistics.
                    Record newRecord = new Record(currentMap, user, scoreProcessor, playTime);
                    lastRecord = newRecord;
                    // Retrieve old records for the map and user.
                    var records = await RecordStore.GetTopRecords(currentMap, user, limit: null, listener: listener?.CreateSubListener<List<IRecord>>());

                    // Save as cleared play.
                    if (scoreProcessor.IsFinished)
                    {
                        RecordStore.SaveRecord(newRecord);

                        var bestRecord = records == null || records.Count == 0 ? null : records[0];
                        userStats.RecordPlay(newRecord, bestRecord);
                    }
                    // Save as failed play.
                    else
                    {
                        userStats.RecordIncompletePlay(newRecord);
                    }
                    listener?.SetFinished(newRecord);
                    return newRecord;
                }
                catch (Exception e)
                {
                    Logger.LogError($"Error while recording score: {e.Message}\n{e.StackTrace}");
                    listener?.SetFinished();
                    return null;
                }
            });
        }

        /// <summary>
        /// Saves the specified replay data with association to the current record.
        /// </summary>
        public void SaveReplay(FileInfo replayFile)
        {
            if (replayFile == null || lastRecord == null || !lastRecord.IsClear)
                return;

            var replayFileDest = RecordStore.GetReplayFile(lastRecord);
            replayFile.MoveTo(replayFileDest.FullName);

            lastRecord.ReplayVersion = currentModeService.LatestReplayVersion;
            RecordStore.SaveRecord(lastRecord);
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

            currentParameter = null;
            currentModeService = null;
            lastRecord = null;
        }

        /// <summary>
        /// Queues a new game loader which handles Unity Resource collection.
        /// </summary>
        private void CleanUpResources()
        {
            AddAsLoader(new AsyncOperationTask(() => Resources.UnloadUnusedAssets()));
        }

        /// <summary>
        /// Initializes a new game session and starts loading the game.
        /// </summary>
        private void InitSession(GameParameter parameter)
        {
            if(currentSession != null)
                throw new InvalidOperationException("Attempted to initialize a redundant game session.");

            currentSession = currentModeService.GetSession(Screen, Dependency);
            currentSession.SetParameter(parameter);
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
                GC.Collect();

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
                    Type = NotificationType.Error
                });
                loadState.Value = GameLoadState.Fail;
                return false;
            }
            if (modeService == null)
            {
                NotificationBox?.Add(new Notification()
                {
                    Message = "Game mode is not specified!",
                    Type = NotificationType.Error
                });
                loadState.Value = GameLoadState.Fail;
                return false;
            }
            return true;
        }
    }
}