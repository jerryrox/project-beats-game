using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Game;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Notifications;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
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


        public bool IsGameLoaded { get; private set; }

        protected override int ScreenDepth => ViewDepths.GameScreen;

        protected override bool IsRoot3D => true;

        [ReceivesDependency]
        private INotificationBox NotificationBox { get; set; }


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

            curSession = modeService.GetSession(this);
            curSession.InvokeHardInit();

            // Wait for pending initial loaders.
            var promise = gameState.GetInitialLoadPromise();
            promise.OnFinished += () =>
            {
                IsGameLoaded = true;
                OnPreInit?.Invoke(true);
            };
            promise.Start();
        }

        public void StartInitialGame()
        {
            curSession.InvokeSoftInit();
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            if (curSession != null)
                curSession.InvokeHardDispose();
            curSession = null;

            CleanupState();
        }

        /// <summary>
        /// Cleans state for clean setup next initialization.
        /// </summary>
        private void CleanupState()
        {
            gameState.Reset();
            IsGameLoaded = false;
        }
    }
}