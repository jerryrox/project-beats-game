using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Offsets
{
    /// <summary>
    /// Toggle which can change the metronome's beat frequency mode.
    /// </summary>
    public class MetronomeMode : LabelledToggle {

        private BeatFrequency frequency = BeatFrequency.Full;


        /// <summary>
        /// The frequency mode this object holds.
        /// </summary>
        public BeatFrequency Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                LabelText = $"x{(int)value}";
            }
        }

        /// <summary>
        /// Returns the current metronome being referenced.
        /// </summary>
        public IMetronome CurMetronome { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            OnTriggered += () =>
            {
                if(CurMetronome != null)
                    CurMetronome.Frequency.Value = frequency;
            };
        }

        /// <summary>
        /// Sets the metronome instance to listen to.
        /// </summary>
        public void SetMetronome(IMetronome metronome)
        {
            RemoveMetronome();

            CurMetronome = metronome;
            if(metronome != null)
                metronome.Frequency.BindAndTrigger(OnFrequencyChange);
        }

        /// <summary>
        /// Removes current metronome association
        /// </summary>
        public void RemoveMetronome()
        {
            if(CurMetronome != null)
                CurMetronome.Frequency.OnNewValue -= OnFrequencyChange;
            CurMetronome = null;
        }

        /// <summary>
        /// Event called on beat frequency change.
        /// </summary>
        private void OnFrequencyChange(BeatFrequency frequency)
        {
            SetFocused(frequency == this.frequency, true);
        }
    }
}