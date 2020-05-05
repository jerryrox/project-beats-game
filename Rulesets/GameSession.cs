using System;
using System.Linq;
using PBGame.UI.Navigations.Screens;
using PBGame.Rulesets.UI;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Scoring;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets
{
    // TODO: Integrate music controller.
    public abstract class GameSession<T> : IGameSession<T>
        where T : HitObject
    {
        public event Action OnHardInit;
        public event Action OnSoftInit;
        public event Action OnSoftDispose;
        public event Action OnHardDispose;
        public event Action OnPause;
        public event Action OnResume;
        public event Action OnRetry;
        public event Action OnForceQuit;
        public event Action OnCompletion;
        public event Action OnFailure;

        private IGraphicObject containerObject;


        public IPlayableMap CurrentMap { get; protected set; }

        public IScoreProcessor ScoreProcessor { get; private set; }

        public IGameGui GameGui { get; private set; }

        /// <summary>
        /// Dependencies of the game session.
        /// Inject this dependency instead when creating the game gui to gain access to this game session instance.
        /// </summary>
        protected IDependencyContainer Dependencies { get; private set; }

        [ReceivesDependency]
        private IGameScreen GameScreen { get; set; }


        protected GameSession(IGraphicObject container)
        {
            if(container == null) throw new ArgumentNullException(nameof(container));

            containerObject = container;
        }

        [InitWithDependency]
        private void Init(IDependencyContainer dependencies)
        {
            this.Dependencies = dependencies.Clone();
            this.Dependencies.CacheAs<IGameSession<T>>(this);
            this.Dependencies.CacheAs<IGameSession>(this);

            // Create game gui.
            GameGui = CreateGameGui(containerObject, this.Dependencies);
            {
                GameGui.Anchor = AnchorType.Fill;
                GameGui.RawSize = Vector2.zero;
            }
        }

        public void SetMap(IPlayableMap map)
        {
            CurrentMap = map;
        }

        public void InvokeHardInit()
        {
            OnHardInit?.Invoke();
        }

        public void InvokeSoftInit()
        {
            // Initialize score processor
            ScoreProcessor = CreateScoreProcessor();

            OnSoftInit?.Invoke();
        }

        public void InvokeSoftDispose()
        {
            int playTime = 0; // TODO: Replace with MusicController time.
            if (ScoreProcessor.JudgeCount > 0)
            {
                // playTime -= (int)(CurrentMap.HitObjects.First().StartTime / 1000f);
                // TODO: Modify play time if using time shift mods.
            }

            // Record score.
            var recordPromise = GameScreen?.RecordScore(ScoreProcessor, playTime);
            recordPromise.OnFinished += () =>
            {
                // Dispose score processor.
                ScoreProcessor = null;

                OnSoftDispose?.Invoke();
            };
            recordPromise.Start();
        }

        public void InvokeHardDispose()
        {
            CurrentMap = null;

            OnHardDispose?.Invoke();
        }

        public void InvokePause()
        {
            // TODO: Pause music, show pause overlay.

            OnPause?.Invoke();
        }

        public void InvokeResume()
        {
            // TODO: Unpause music

            OnResume?.Invoke();
        }

        public void InvokeRetry()
        {
            // TODO: Game GUI fade out
            // TODO: OnRetry event emit & Soft dispose -> Soft init
            // TODO: Game GUI fade in
        }

        public void InvokeForceQuit()
        {
            // TODO: SoftDispose -> GameScreen.ExitGame<PrepareScreen>() & OnForceQuit event emit;
            InvokeSoftDispose();
        }

        public void InvokeCompletion()
        {
            // TODO: SoftDispose -> Game GUI fade out & OnCompletion event emit
            // TODO: Either wait for 3 seconds or listen to escape key trigger.
            // TODO: GameScreen.ExitGame<ResultScreen>()
        }

        public void InvokeFailure()
        {
            OnFailure?.Invoke();

            // TODO: Failure effect -> Failure overlay
            // TODO: If retry, SotDispose -> SoftInit
            // TODO: If exit, SoftDispose -> GameScreen.ExitGame<PrepareScreen>()
        }

        /// <summary>
        /// Creates a new instance of the game gui under the specified container object.
        /// </summary>
        protected abstract IGameGui CreateGameGui(IGraphicObject container, IDependencyContainer dependencies);

        /// <summary>
        /// Creates a new instance of the score processor.
        /// </summary>
        protected abstract IScoreProcessor CreateScoreProcessor();


        /// <summary>
        /// Types of game disposal action.
        /// </summary>
        protected enum DisposeType
        {
            ForceQuit = 0,
            Complete,
            Retry,
        }
    }
}