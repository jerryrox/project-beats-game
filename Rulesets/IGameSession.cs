using System;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Scoring;

namespace PBGame.Rulesets
{
    public interface IGameSession
    {
        /// <summary>
        /// Event called when the user has successfully cleared the map.
        /// </summary>
        event Action OnGameClear;

        /// <summary>
        /// Event called when the user has failed the map.
        /// </summary>
        event Action OnGameFail;

        /// <summary>
        /// Event called when the game is about to be disposed.
        /// </summary>
        event Action OnGameDispose;


        /// <summary>
        /// Current map in play.
        /// </summary>
        IMap CurrentMap { get; }

        /// <summary>
        /// The score processor of current game session.
        /// </summary>
        IScoreProcessor ScoreProcessor { get; }


        /// <summary>
        /// Initializes the session for a new game.
        /// </summary>
        void InitSession();
    }

    public interface IGameSession<T>
        where T : HitObject
    {
    }
}