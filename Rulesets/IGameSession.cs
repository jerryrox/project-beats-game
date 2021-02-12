using System;
using PBGame.Stores;
using PBGame.Rulesets.UI;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Scoring;

namespace PBGame.Rulesets
{
    public interface IGameSession
    {
        /// <summary>
        /// Event called when a new game session is about to be loaded.
        /// </summary>
        event Action OnHardInit;

        /// <summary>
        /// Event called every time when the game needs to be reset to initial values for a play within session.
        /// </summary>
        event Action OnSoftInit;

        /// <summary>
        /// Event called every time when the game is stopped forcibly or gracefully for a cleanup.
        /// </summary>
        event Action OnSoftDispose;

        /// <summary>
        /// Event called when current game session is about to be unloaded completely.
        /// </summary>
        event Action OnHardDispose;

        /// <summary>
        /// Event called on game pause due to player triggering pause.
        /// </summary>
        event Action OnPause;

        /// <summary>
        /// Event called on game resume after unpausing the game.
        /// </summary>
        event Action OnResume;

        /// <summary>
        /// Event called on retrying the game.
        /// </summary>
        event Action OnRetry;

        /// <summary>
        /// Event called on forcibly resetting the game.
        /// </summary>
        event Action OnForceQuit;

        /// <summary>
        /// Event called on a full completion of the game.
        /// </summary>
        event Action OnCompletion;

        /// <summary>
        /// Event called on user pressing the skip button.
        /// </summary>
        event Action<float> OnSkipped;


        /// <summary>
        /// Returns the current game processor managing the gameplay.
        /// </summary>
        GameProcessor GameProcessor { get; }

        /// <summary>
        /// The current parameter being used to play the session.
        /// </summary>
        GameParameter CurrentParameter { get; }

        /// <summary>
        /// Returns the store for loading map assets.
        /// </summary>
        MapAssetStore MapAssetStore { get; }

        /// <summary>
        /// Returns the current map in play.
        /// </summary>
        IPlayableMap CurrentMap { get; }

        /// <summary>
        /// Returns the score processor of current game session.
        /// </summary>
        IScoreProcessor ScoreProcessor { get; }

        /// <summary>
        /// Returns the gui part of the game.
        /// </summary>
        GameGui GameGui { get; }

        /// <summary>
        /// Returns the amount of time in MS to delay before playing the music.
        /// </summary>
        float LeadInTime { get; }

        /// <summary>
        /// Returns whether the game session is currently playing.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Returns whether the game is currently paused.
        /// </summary>
        bool IsPaused { get; }


        /// <summary>
        /// Returns the duration of play in seconds.
        /// </summary>
        int GetPlayTime();

        /// <summary>
        /// Sets the parameter of the gameplay session.
        /// </summary>
        void SetParameter(GameParameter parameter);

        /// <summary>
        /// Invokes hard initialization event.
        /// </summary>
        void InvokeHardInit();

        /// <summary>
        /// Invokes soft initialization event.
        /// </summary>
        void InvokeSoftInit();

        /// <summary>
        /// Invokes soft disposal event.
        /// </summary>
        void InvokeSoftDispose();

        /// <summary>
        /// Invokes hard disposal event.
        /// </summary>
        void InvokeHardDispose();

        /// <summary>
        /// Invokes pause event.
        /// </summary>
        void InvokePause();

        /// <summary>
        /// Invokes resume event.
        /// </summary>
        void InvokeResume();

        /// <summary>
        /// Invokes retry event.
        /// </summary>
        void InvokeRetry();

        /// <summary>
        /// Invokes force quit event.
        /// </summary>
        void InvokeForceQuit();

        /// <summary>
        /// Invokes play completion event.
        /// </summary>
        void InvokeCompletion();

        /// <summary>
        /// Invokes skipped event.
        /// </summary>
        void InvokeSkipped(float time);
    }

    public interface IGameSession<T> : IGameSession
        where T : BaseHitObject
    {
    }
}