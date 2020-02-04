using System;

namespace PBGame.Audio
{
    public interface IMetronome {

        /// <summary>
        /// Event called when the metronome has reached the next beat.
        /// </summary>
        event Action OnBeat;

        /// <summary>
        /// Event called when the beat length has been changed.
        /// </summary>
        event Action<double> OnBeatLengthChange;


        /// <summary>
        /// Returns the length of a beat in milliseconds.
        /// </summary>
        double BeatLength { get; }


        /// <summary>
        /// Updates the metronome.
        /// </summary>
        void Update();
    }
}