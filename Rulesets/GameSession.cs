using System;
using System.Linq;
using PBGame.UI.Navigations.Screens;
using PBGame.Rulesets.UI;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Scoring;
using PBFramework.UI.Navigations;
using PBFramework.Data;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets
{
    // TODO: Integrate music controller.
    public abstract class GameSession<T> : IGameSession<T>
        where T : BaseHitObject
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

        private IGraphicObject containerObject;


        public IPlayableMap CurrentMap { get; protected set; }

        public IScoreProcessor ScoreProcessor { get; private set; }

        public GameGui GameGui { get; private set; }

        public float LeadInTime => Mathf.Max(2000f, CurrentMap.Detail.AudioLeadIn);

        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Dependencies of the game session.
        /// Inject this dependency instead when creating the game gui to gain access to this game session instance.
        /// </summary>
        protected IDependencyContainer Dependencies { get; private set; }

        /// <summary>
        /// Returns the game screen instance.
        /// </summary>
        private GameScreen GameScreen => ScreenNavigator.Get<GameScreen>();

        [ReceivesDependency]
        protected IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


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
                GameGui.Offset = Offset.Zero;
            }
        }

        public void SetMap(IPlayableMap map)
        {
            CurrentMap = map;
        }

        public int GetPlayTime()
        {
            int playTime = (int)MusicController.Clock.CurrentTime;
            if (ScoreProcessor.JudgeCount > 0)
            {
                playTime -= (int)(CurrentMap.HitObjects.First().StartTime / 1000f);
                // TODO: Modify play time if using time shift mods.
            }
            return playTime;
        }

        public void InvokeHardInit()
        {
            IsPlaying = false;
            
            OnHardInit?.Invoke();
        }

        public void InvokeSoftInit()
        {
            // Initialize score processor
            ScoreProcessor = CreateScoreProcessor();
            ScoreProcessor.ApplyMap(CurrentMap);
            ScoreProcessor.OnLastJudgement += InvokeCompletion;

            MusicController.Stop();
            MusicController.SetFade(1f);

            // Show game gui.
            GameGui.ShowGame(() =>
            {
                MusicController.Play(LeadInTime);

                IsPlaying = true;

                OnSoftInit?.Invoke();
            });
        }

        public void InvokeSoftDispose()
        {
            IsPlaying = false;

            int playTime = GetPlayTime();

            // Record score.
            var recordPromise = GameScreen?.RecordScore(ScoreProcessor, playTime);
            recordPromise.OnFinished += () =>
            {
                // Dispose score processor.
                ScoreProcessor = null;

                // Hide game gui
                GameGui.HideGame(() => OnSoftDispose?.Invoke());
            };
            recordPromise.Start();
        }

        public void InvokeHardDispose()
        {
            IsPlaying = false;
            CurrentMap = null;

            OnHardDispose?.Invoke();
        }

        public void InvokePause()
        {
            if(MusicController.IsPlaying)
                MusicController.Pause();

            // TODO: show pause overlay.

            OnPause?.Invoke();
        }

        public void InvokeResume()
        {
            if(MusicController.IsPaused)
                MusicController.Play();

            OnResume?.Invoke();
        }

        public void InvokeRetry()
        {
            EventBinder<Action> onDispose = new EventBinder<Action>(e => OnSoftDispose += e, e => OnSoftDispose -= e);
            onDispose.IsOneTime = true;
            onDispose.SetHandler(() =>
            {
                OnRetry?.Invoke();
                InvokeSoftInit();
            });
            InvokeSoftDispose();
        }

        public void InvokeForceQuit()
        {
            EventBinder<Action> onDispose = new EventBinder<Action>(e => OnSoftDispose += e, e => OnSoftDispose -= e);
            onDispose.IsOneTime = true;
            onDispose.SetHandler(() =>
            {
                OnForceQuit?.Invoke();
                GameScreen.ExitGame<PrepareScreen>();
            });
            InvokeSoftDispose();
        }

        public void InvokeCompletion()
        {
            EventBinder<Action> onDispose = new EventBinder<Action>(e => OnSoftDispose += e, e => OnSoftDispose -= e);
            onDispose.IsOneTime = true;
            onDispose.SetHandler(() =>
            {
                OnCompletion?.Invoke();

                // TODO: Wait for completion timeout to auto navigate to results.
                SynchronizedTimer timer = new SynchronizedTimer()
                {
                    Limit = 2f,
                };
                timer.OnFinished += delegate { GameScreen.ExitGame<PrepareScreen>(); };
                timer.Start();

                // TODO: Navigate to ResultScreen.
                // GameScreen.ExitGame<ResultScreen>();
            });
            InvokeSoftDispose();
        }

        /// <summary>
        /// Creates a new instance of the game gui under the specified container object.
        /// </summary>
        protected abstract GameGui CreateGameGui(IGraphicObject container, IDependencyContainer dependencies);

        /// <summary>
        /// Creates a new instance of the score processor.
        /// </summary>
        protected abstract IScoreProcessor CreateScoreProcessor();
    }
}