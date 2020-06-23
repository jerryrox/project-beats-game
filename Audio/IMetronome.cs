using System;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBFramework.Audio;
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
        /// The current map which the metronome is referring to.
        /// </summary>
        IPlayableMap CurrentMap { get; set; }

        /// <summary>
        /// The audio controller to refer current time value from.
        /// </summary>
        IAudioController AudioController { get; set; }

        /// <summary>
        /// Returns the index of the beat within the current interval.
        /// </summary>
        IReadOnlyBindable<int> BeatIndex { get; }

        /// <summary>
        /// Returns the number of beats in the interval of current time signature.
        /// </summary>
        IReadOnlyBindable<int> BeatsInInterval { get; }

        /// <summary>
        /// Current beat frequency scale.
        /// </summary>
        Bindable<BeatFrequency> Frequency { get; }

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