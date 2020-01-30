namespace PBGame.UI.Components.Home
{
    public interface ILogoDisplay : Components.ILogoDisplay {

        /// <summary>
        /// The duration of a single pulse.
        /// </summary>
        float PulseDuration { get; set; }


        /// <summary>
        /// Sets the progress of the pulse.
        /// </summary>
        void SetPulseProgress(float progress);

        /// <summary>
        /// Starts playing the pulsating animation.
        /// </summary>
        void PlayPulse();

        /// <summary>
        /// Stops playing the pulsating animation.
        /// </summary>
        void StopPulse();
    }
}