using System;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
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
    }
}