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
        public event Action OnGameInit;
        public event Action OnGameClear;
        public event Action OnGameFail;
        public event Action OnGameDispose;

        private IGraphicObject containerObject;


        public IMap CurrentMap { get; protected set; }

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

            // Create game gui.
            GameGui = CreateGameGui(containerObject, this.Dependencies);
            {
                GameGui.Anchor = Anchors.Fill;
                GameGui.RawSize = Vector2.zero;
            }
        }

        public void SetMap(IMap map)
        {
            CurrentMap = map;
        }

        public void InitSession()
        {
            // Initialize score processor
            ScoreProcessor = CreateScoreProcessor();
            // TODO: Listen to events

            OnGameInit?.Invoke();
        }

        public void DisposeSession()
        {
            // Dispose score processor
            // TODO: Remove events
            ScoreProcessor = null;

            OnGameDispose?.Invoke();
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