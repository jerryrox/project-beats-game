using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Game;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Data.Rankings;
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

namespace PBGame.UI.Navigations.Screens
{
    public class GameScreen : BaseScreen, IGameScreen {

        public event Action<bool> OnPreInit;

        private GameState gameState;
        private IPlayableMap curMap;
        private IModeService curMode;
        private IGameSession curSession;
        private IRecord newRecord;


        public bool IsGameLoaded { get; private set; }

        public bool IsLoadSuccess { get; private set; }

        protected override int ScreenDepth => ViewDepths.GameScreen;

        protected override bool IsRoot3D => true;

        [ReceivesDependency]
        private INotificationBox NotificationBox { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

        [ReceivesDependency]
        private IRecordManager RecordManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Dependencies = Dependencies.Clone();
            Dependencies.Cache(gameState = new GameState());
        }

        public void PreInitialize(IPlayableMap map, IModeService modeService)
        {
            CleanupState();

            if (map == null)
            {
                NotificationBox?.Add(new Notification()
                {
                    Message = "Map is not specified!",
                    Type = NotificationType.Negative
                });
                OnPreInit?.Invoke(false);
                return;
            }
            if (modeService == null)
            {
                NotificationBox?.Add(new Notification()
                {
                    Message = "Game mode is not specified!",
                    Type = NotificationType.Negative
                });
                OnPreInit?.Invoke(false);
                return;
            }

            curMap = map;
            curMode = modeService;

            SetSession(modeService.GetSession(this));

            // Wait for pending initial loaders.
            var promise = gameState.GetInitialLoadPromise();
            promise.OnFinished += () =>
            {
                IsGameLoaded = true;
                IsLoadSuccess = true;
                OnPreInit?.Invoke(true);
            };
            promise.Start();
        }

        public void StartInitialGame()
        {
            curSession.InvokeSoftInit();
        }

        public void ExitGame<T>()
            where T : BaseScreen
        {
            var screen = ScreenNavigator.Show<T>();
            // TODO: If Result screen, pass the newRecord object to ResultScreen.
            // if(screen is re
        }

        public IExplicitPromise RecordScore(IScoreProcessor scoreProcessor, int playTime)
        {
            if(scoreProcessor == null || scoreProcessor.JudgeCount <= 0)
                return new ProxyPromise();

            // Retrieve user and user stats.
            var user = UserManager.CurrentUser.Value;
            if(user == null)
                return new ProxyPromise();
            var userStats = user.GetStatistics(curMap.PlayableMode);
            if (userStats == null)
                return new ProxyPromise();

            // Record the play result to records database and user statistics.
            newRecord = new Record(curMap, user, scoreProcessor, playTime);
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
                RecordManager.GetRecords(curMap, recordProgress);
            });
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            ClearSession();
            CleanupState();
        }

        /// <summary>
        /// Cleans state for clean setup next initialization.
        /// </summary>
        private void CleanupState()
        {
            gameState.Reset();
            IsGameLoaded = false;
            IsLoadSuccess = false;
            newRecord = null;
        }

        /// <summary>
        /// Sets the current game session to bind on.
        /// </summary>
        private void SetSession(IGameSession gameSession)
        {
            ClearSession();

            curSession = gameSession;
            curSession.InvokeHardInit();
        }

        /// <summary>
        /// Removes bind with current game session if exists.
        /// </summary>
        private void ClearSession()
        {
            if(curSession == null)
                return;

            curSession.InvokeHardDispose();
            curSession = null;
        }
    }
}