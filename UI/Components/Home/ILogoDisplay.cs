using System;

namespace PBGame.UI.Components.Home
{
    public interface ILogoDisplay : Components.ILogoDisplay {

        /// <summary>
        /// Event caled on logo press.
        /// </summary>
        event Action OnPress;

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

        /// <summary>
        /// Sets zoom effect on the logo.
        /// </summary>
        void SetZoom(bool isZoom);
    }
}