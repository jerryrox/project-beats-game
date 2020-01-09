using System;
using PBGame.Rulesets.UI;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Scoring;

namespace PBGame.Rulesets
{
    public interface IGameSession
    {
        /// <summary>
        /// Event called when the game session is initializing.
        /// </summary>
        event Action OnGameInit;

        /// <summary>
        /// Event called when the game is about to be disposed.
        /// </summary>
        event Action OnGameDispose;

        /// <summary>
        /// Event called when the user has successfully cleared the map.
        /// </summary>
        event Action OnGameClear;

        /// <summary>
        /// Event called when the user has failed the map.
        /// </summary>
        event Action OnGameFail;


        /// <summary>
        /// Returns the current map in play.
        /// </summary>
        IMap CurrentMap { get; }

        /// <summary>
        /// Returns the score processor of current game session.
        /// </summary>
        IScoreProcessor ScoreProcessor { get; }

        /// <summary>
        /// Returns the gui part of the game.
        /// </summary>
        IGameGui GameGui { get; }


        /// <summary>
        /// Sets the map to play.
        /// </summary>
        void SetMap(IMap map);

        /// <summary>
        /// Initializes the session for a new game.
        /// </summary>
        void InitSession();

        /// <summary>
        /// Disposes current session.
        /// </summary>
        void DisposeSession();
    }

    public interface IGameSession<T> : IGameSession
        where T : HitObject
    {
    }
}