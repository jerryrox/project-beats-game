using System;
using PBFramework.Data.Bindables;

namespace PBGame.Audio
{
    public interface IMetronome {

        /// <summary>
        /// Event called when the metronome has reached the next beat.
        /// Passes the current beat index value.
        /// </summary>
        event Action OnBeat;


        /// <summary>
        /// Returns the index of the beat within the current interval.
        /// </summary>
        IReadOnlyBindable<int> BeatIndex { get; }

        /// <summary>
        /// Returns the number of beats in the interval of current time signature.
        /// </summary>
        int BeatsInInterval { get; }

        /// <summary>
        /// Current beat frequency scale.
        /// </summary>
        BeatFrequency Frequency { get; set; }

        /// <summary>
        /// Returns the length of a beat in milliseconds.
        /// </summary>
        IReadOnlyBindable<float> BeatLength { get; }


        /// <summary>
        /// Updates the metronome.
        /// </summary>
        void Update();
    }
}