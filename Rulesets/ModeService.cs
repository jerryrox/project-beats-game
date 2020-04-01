using PBGame.Rulesets.Difficulty;
using PBGame.Rulesets.Judgements;
using PBGame.Rulesets.Maps;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets
{
	/// <summary>
	/// Provides various mode-specific functions and processes.
	/// </summary>
	public abstract class ModeService : IModeService {

		/// <summary>
		/// Game session last created, if exists.
		/// </summary>
		protected IGameSession cachedSession;

		private IDependencyContainer dependencyContainer;

		
        public abstract string Name { get; }

		public abstract string IconName { get; }

        public abstract GameModes GameMode { get; }

        public virtual bool IsPlayable => true;


        protected ModeService(IDependencyContainer dependencyContainer)
		{
			this.dependencyContainer = dependencyContainer;
		}

		public abstract IMapConverter CreateConverter(IOriginalMap map);

		public abstract IMapProcessor CreateProcessor(IPlayableMap map);

		public abstract IDifficultyCalculator CreateDifficultyCalculator(IPlayableMap map);

		public abstract HitTiming CreateTiming();

		public IGameSession GetSession(IGraphicObject container)
		{
			if(cachedSession == null)
			{
				cachedSession = CreateSession(container);
				dependencyContainer.Inject(cachedSession);
			}
			return cachedSession;
		}

		/// <summary>
		/// Creates a new instance of the game session for current mode.
		/// </summary>
		protected abstract IGameSession CreateSession(IGraphicObject container);
	}
}

