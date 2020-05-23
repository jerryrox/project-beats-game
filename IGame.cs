using System;
using PBFramework.Dependencies;

namespace PBGame
{
    public interface IGame {

        /// <summary>
        /// Event called on application focus/unfocus.
        /// Specifies whether the application is currently focused.
        /// </summary>
        event Action<bool> OnAppFocus;

        /// <summary>
        /// Event called on application pause/resume.
        /// Specifies whether the application is currently paused.
        /// </summary>
        event Action<bool> OnAppPause;


        /// <summary>
        /// Returns the dependencies container initialized from the game context.
        /// </summary>
        IDependencyContainer Dependencies { get; }

        /// <summary>
        /// Returns the version of the game.
        /// </summary>
        string Version { get; }


        /// <summary>
        /// Gracefully quits the game.
        /// </summary>
        void GracefulQuit();

        /// <summary>
        /// Forcefully quits the game.
        /// </summary>
        void ForceQuit();
    }
}