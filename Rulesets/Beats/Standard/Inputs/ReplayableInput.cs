using PBGame.IO;
using PBFramework.Data.Bindables;
using PBFramework.Inputs;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class ReplayableInput : ICursor, IStreamableData
    {
        private static readonly Vector2 DefaultPosition = new Vector2(10000, 10000);

        private Bindable<InputState> state = new Bindable<InputState>();
        private BindableBool isActive = new BindableBool();


        /// <summary>
        /// The time of input in milliseconds.
        /// </summary>
        public float Time { get; set; }

        public KeyCode Key { get; set; }

        public IReadOnlyBindable<InputState> State => state;

        public IReadOnlyBindable<bool> IsActive => isActive;

        public Vector2 RawPosition { get; set; }

        public Vector2 RawDelta { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Delta { get; set; }


        /// <summary>
        /// Sets the attributes based on the specified cursor.
        /// </summary>
        public void SetFromCursor(float time, ICursor cursor)
        {
            Time = time;
            Key = cursor.Key;
            state.Value = cursor.State.Value;
            isActive.Value = cursor.IsActive.Value;
            RawPosition = cursor.RawPosition;
            RawDelta = cursor.RawDelta;
            Position = cursor.Position;
            Delta = cursor.Delta;
        }

        /// <summary>
        /// Sets the attributes based on the specified input (most likely a key).
        /// </summary>
        public void SetFromInput(float time, IInput input)
        {
            Time = time;
            Key = input.Key;
            state.Value = input.State.Value;
            isActive.Value = input.IsActive.Value;
            RawPosition = DefaultPosition;
            RawDelta = DefaultPosition;
            Position = DefaultPosition;
            Delta = DefaultPosition;
        }

        public string ToStreamData()
        {
            return $"{Time};{Key};{state.Value};{(isActive.Value ? 1 : 0)};{RawPosition.x},{RawPosition.y};{RawDelta.x},{RawDelta.y};{Position.x},{Position.y};{Delta.x},{Delta.y}\n";
        }

        void IInput.Release() {}

        void IInput.SetActive(bool active) { }

        void ICursor.SetResolution(Vector2 resolution) { }
    }
}