using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;
using PBGame.Rulesets.Difficulty;
using PBGame.Rulesets.Judgements;
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

		
        public abstract string Name { get; }

		public abstract string BaseIconName { get; }

        public abstract GameModeType GameMode { get; }

        public virtual bool IsPlayable => true;
		

        public string GetIconName(int size) => $"{BaseIconName}-{size}";

        public abstract IMapConverter CreateConverter(IOriginalMap map);

		public abstract IMapProcessor CreateProcessor(IPlayableMap map);

		public abstract IDifficultyCalculator CreateDifficultyCalculator(IPlayableMap map);

        public abstract IScoreProcessor CreateScoreProcessor();

        public abstract HitTiming CreateTiming();

		public IGameSession GetSession(IGraphicObject container, IDependencyContainer dependency)
		{
            if(cachedSession == null)
			{
				cachedSession = CreateSession(container, dependency);
			}
			return cachedSession;
		}

		/// <summary>
		/// Creates a new instance of the game session for current mode.
		/// </summary>
		protected abstract IGameSession CreateSession(IGraphicObject container, IDependencyContainer dependency);
	}
}

