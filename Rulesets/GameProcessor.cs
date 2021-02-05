using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets
{
    public abstract class GameProcessor : UguiObject {

        /// <summary>
        /// Returns the current time of the music.
        /// </summary>
        public abstract float CurrentTime { get; }

        /// <summary>
        /// Returns the parameter of the game session.
        /// </summary>
        protected GameParameter GameParameter => GameSession.CurrentParameter;

        [ReceivesDependency]
        protected IGameSession GameSession { get; set; }

        [ReceivesDependency]
        protected IMusicController MusicController { get; set; }

    }
}