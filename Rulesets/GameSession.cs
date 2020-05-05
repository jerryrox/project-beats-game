using System;
using PBGame.Rulesets.UI;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Scoring;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets
{
    public abstract class GameSession<T> : IGameSession<T>
        where T : HitObject
    {
        public event Action OnHardInit;
        public event Action OnSoftInit;
        public event Action OnSoftDispose;
        public event Action OnHardDispose;
        public event Action OnPause;
        public event Action OnResume;
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
            // TODO: Record score if at least one judgement has been made.

            // Dispose score processor.
            ScoreProcessor = null;

            OnSoftDispose?.Invoke();
        }

        public void InvokeHardDispose()
        {
            OnHardDispose?.Invoke();
        }

        public void InvokePause()
        {
            // TODO: Pause music

            OnPause?.Invoke();
        }

        public void InvokeResume()
        {
            // TODO: Unpause music

            OnResume?.Invoke();
        }

        public void InvokeForceQuit()
        {


            InvokeSoftDispose();
            OnForceQuit?.Invoke();
        }

        public void InvokeCompletion()
        {
            // TODO: 

            OnCompletion?.Invoke();
        }

        public void InvokeFailure()
        {
            // TODO: Display failure overlay

            OnFailure?.Invoke();
        }

        /// <summary>
        /// Creates a new instance of the game gui under the specified container object.
        /// </summary>
        protected abstract IGameGui CreateGameGui(IGraphicObject container, IDependencyContainer dependencies);

        /// <summary>
        /// Creates a new instance of the score processor.
        /// </summary>
        protected abstract IScoreProcessor CreateScoreProcessor();
    }
}