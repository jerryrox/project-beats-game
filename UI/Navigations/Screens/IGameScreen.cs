using System;
using PBGame.UI.Navigations.Screens;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;
using PBFramework;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;

namespace PBGame.UI.Navigations.Screens
{
    public interface IGameScreen : INavigationView {

        /// <summary>
        /// Event called on pre initialization finish.
        /// </summary>
        event Action<bool> OnPreInit;

        /// <summary>
        /// Event called on escape key or gesture.
        /// </summary>
        event Action OnEscape;


        /// <summary>
        /// Returns whether the game session is fully loaded.
        /// </summary>
        bool IsGameLoaded { get; }


        /// <summary>
        /// Starts initializing the game using the specified playable map and mode servicer.
        /// </summary>
        void PreInitialize(IPlayableMap map, IModeService modeService);

        /// <summary>
        /// Starts the game initially after game load overlay.
        /// </summary>
        void StartInitialGame();

        /// <summary>
        /// Hard-disposes the current game session and navigates to the screen T.
        /// </summary>
        void ExitGame<T>() where T : BaseScreen;

        /// <summary>
        /// Returns the process which records the specified score details and play time in seconds to user profile.
        /// </summary>
        IExplicitPromise RecordScore(IScoreProcessor scoreProcessor, int playTime);
    }
}