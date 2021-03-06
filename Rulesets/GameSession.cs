using System;
using System.Linq;
using PBGame.UI.Models;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Data.Records;
using PBGame.Audio;
using PBGame.Stores;
using PBGame.Rulesets.UI;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Scoring;
using PBGame.Rulesets.Judgements;
using PBGame.Configurations;
using PBFramework.UI.Navigations;
using PBFramework.Data;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Threading;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets
{
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
        public event Action<float> OnSkipped;

        protected ManagedRecycler<ReplayableJudgement> replayJudgementsRecycler;
        
        private IGraphicObject containerObject;


        public abstract GameProcessor GameProcessor { get; }

        public GameParameter CurrentParameter { get; private set; }

        public MapAssetStore MapAssetStore { get; private set; }

        public IPlayableMap CurrentMap => CurrentParameter.Map;

        public IScoreProcessor ScoreProcessor { get; private set; }

        public GameGui GameGui { get; private set; }

        public float LeadInTime => Mathf.Max(2000f, CurrentMap.Detail.AudioLeadIn);

        public bool IsPlaying { get; private set; }

        public bool IsPaused { get; private set; }

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
        protected IGame Game { get; set; }

        [ReceivesDependency]
        protected IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private GameModel Model { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private ISoundTable SoundTable { get; set; }

        [ReceivesDependency]
        private ISoundPool SoundPool { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


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

            replayJudgementsRecycler = new ManagedRecycler<ReplayableJudgement>(CreateReplayJudgement);
            this.Dependencies.CacheAs<IRecycler<ReplayableJudgement>>(replayJudgementsRecycler);

            // Create game gui.
            GameGui = CreateGameGui(containerObject, this.Dependencies);
            {
                GameGui.Anchor = AnchorType.Fill;
                GameGui.Offset = Offset.Zero;
            }
        }

        public void SetParameter(GameParameter parameter)
        {
            CurrentParameter = parameter;
            
            // Set map
            MapAssetStore = new MapAssetStore(parameter.Map, SoundTable);
        }

        public int GetPlayTime()
        {
            int playTime = (int)GameProcessor.CurrentTime;
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

            // Load map hitsounds only if enabled in configuration.
            if(GameConfiguration.UseBeatmapHitsounds.Value)
                Model.AddAsLoader(MapAssetStore.LoadHitsounds());
        }

        public void InvokeSoftInit()
        {
            // Initialize score processor
            ScoreProcessor = Model.ModeService.CreateScoreProcessor();
            ScoreProcessor.ApplyMap(CurrentMap);
            ScoreProcessor.OnLastJudgement += InvokeCompletion;

            MusicController.Stop();
            MusicController.SetFade(1f);

            // Show game gui.
            GameGui.ShowGame(() =>
            {
                MusicController.Play(LeadInTime);

                // Start listening to application pause event.
                Game.OnAppPause += OnAppPaused;
                Game.OnAppFocus += OnAppFocused;

                IsPlaying = true;

                OnSoftInit?.Invoke();
            });
        }

        public void InvokeSoftDispose()
        {
            IsPlaying = false;
            IsPaused = false;

            int playTime = GetPlayTime();

            // Stop listening to app pause event.
            Game.OnAppPause -= OnAppPaused;
            Game.OnAppFocus -= OnAppFocused;

            // Record score if not replay mode.
            if (!CurrentParameter.IsReplay)
            {
                var listener = new TaskListener<IRecord>();
                listener.OnFinished += (record) =>
                {
                    ScoreProcessor = null;
                    GameGui.HideGame(() => OnSoftDispose?.Invoke());
                };
                Model?.RecordScore(ScoreProcessor, playTime, listener);
            }
            else
            {
                GameGui.HideGame(() => OnSoftDispose?.Invoke());
            }
        }

        public void InvokeHardDispose()
        {
            IsPlaying = false;

            CurrentParameter = null;

            // Before destroying custom hitsounds in the asset store, they must first be unmounted from the sound pool.
            SoundPool.UnmountAll();
            MapAssetStore.Dispose();
            MapAssetStore = null;

            OnHardDispose?.Invoke();
        }

        public void InvokePause()
        {
            if(IsPaused)
                return;
            if(GameProcessor.CurrentTime < 0f)
                return;
            if(MusicController.IsPlaying)
                MusicController.Pause();

            var pauseOverlay = OverlayNavigator.Show<PauseOverlay>();

            IsPaused = true;
            OnPause?.Invoke();
        }

        public void InvokeResume()
        {
            if(!IsPaused)
                return;
            if(MusicController.IsPaused)
                MusicController.Play();

            IsPaused = false;
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
                Model.ExitGameForceful();
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

                // Wait for completion timeout to auto navigate to results.
                SynchronizedTimer autoExitTimer = new SynchronizedTimer()
                {
                    Limit = 2f,
                };
                autoExitTimer.OnFinished += Model.ExitGameToResult;
                autoExitTimer.Start();
            });

            SynchronizedTimer initialTimer = new SynchronizedTimer()
            {
                Limit = 0.5f
            };
            initialTimer.OnFinished += InvokeSoftDispose;
            initialTimer.Start();
        }

        public void InvokeSkipped(float time)
        {
            MusicController.Seek(time);
            OnSkipped?.Invoke(time);
        }

        /// <summary>
        /// Event called on application pause.
        /// </summary>
        protected virtual void OnAppPaused(bool paused)
        {
            if (paused)
                InvokePause();
        }

        /// <summary>
        /// Event called on application focus.
        /// </summary>
        protected virtual void OnAppFocused(bool focused)
        {
            if (!focused)
                InvokePause();
        }

        /// <summary>
        /// Creates a new instance of the game gui under the specified container object.
        /// </summary>
        protected abstract GameGui CreateGameGui(IGraphicObject container, IDependencyContainer dependencies);

        /// <summary>
        /// Creates a new replayable judgement instance for the recycler.
        /// </summary>
        private ReplayableJudgement CreateReplayJudgement()
        {
            return new ReplayableJudgement();
        }
    }
}