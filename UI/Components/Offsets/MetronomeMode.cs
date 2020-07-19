using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Audio;
using PBFramework.Dependencies;

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
                RefreshFocus();
            }
        }

        [ReceivesDependency]
        private OffsetsModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnTriggered += () => Model.SetFrequency(frequency);

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            if(Model != null)
                Model.Metronome.Frequency.BindAndTrigger(OnFrequencyChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if(Model != null)
                Model.Metronome.Frequency.OnNewValue -= OnFrequencyChange;
        }

        /// <summary>
        /// Refreshes focused state.
        /// </summary>
        private void RefreshFocus()
        {
            if(Model == null)
                SetFocused(!IsFocused, true);
            else
                SetFocused(this.frequency == Model.Metronome.Frequency.Value, true);
        }

        /// <summary>
        /// Event called on beat frequency change.
        /// </summary>
        private void OnFrequencyChange(BeatFrequency frequency) => RefreshFocus();
    }
}