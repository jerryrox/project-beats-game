using PBFramework.Dependencies;

namespace PBGame
{
    public interface IGame {
    
        /// <summary>
        /// Returns the dependencies container initialized from the game context.
        /// </summary>
        IDependencyContainer Dependencies { get; }


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