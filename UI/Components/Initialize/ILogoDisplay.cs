using System;

namespace PBGame.UI.Components.Initialize
{
    public interface ILogoDisplay : Components.ILogoDisplay {

        /// <summary>
        /// Event called when the startup animation has finished.
        /// </summary>
        Action OnStartup { get; set; }

        /// <summary>
        /// Event called when the end animation has finished.
        /// </summary>
        Action OnEnd { get; set; }


        /// <summary>
        /// Plays the initial logo animation.
        /// </summary>
        void PlayStartup();

        /// <summary>
        /// Plays the looping animation on the logo.
        /// </summary>
        void PlayBreathe();

        /// <summary>
        /// Plays the logo hide animation.
        /// </summary>
        void PlayEnd();
    }
}