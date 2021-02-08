using System;
using PBGame.IO;
using PBFramework.Data.Bindables;
using PBFramework.Inputs;
using PBFramework.Allocation.Recyclers;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class ReplayableInput : ICursor, IStreamableData, IRecyclable<ReplayableInput>
    {
        private static readonly Vector2 DefaultPosition = new Vector2(10000, 10000);

        private Bindable<InputState> state = new Bindable<InputState>();
        private BindableBool isActive = new BindableBool();


        public KeyCode Key { get; set; }

        public IReadOnlyBindable<InputState> State => state;

        public IReadOnlyBindable<bool> IsActive => isActive;

        public Vector2 RawPosition { get; set; }

        public Vector2 RawDelta { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Delta { get; set; }

        IRecycler<ReplayableInput> IRecyclable<ReplayableInput>.Recycler { get; set; }


        /// <summary>
        /// Sets the attributes based on the specified cursor.
        /// </summary>
        public void SetFromCursor(ICursor cursor)
        {
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
        public void SetFromInput(IInput input)
        {
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
            return $"{(int)Key};{(int)state.Value};{(isActive.Value ? 1 : 0)};{RawPosition.x},{RawPosition.y};{RawDelta.x},{RawDelta.y};{Position.x},{Position.y};{Delta.x},{Delta.y}";
        }

        public void FromStreamData(string data)
        {
            if (data.Length == 0)
                return;

            Func<string, Vector2> parseVector = (string rawData) =>
            {
                int commaInx = rawData.IndexOf(',');
                return new Vector2(float.Parse(rawData.Substring(0, commaInx)), float.Parse(rawData.Substring(commaInx + 1)));
            };

            string[] segments = data.Split(';');
            int i = 0;
            Key = (KeyCode)int.Parse(segments[i++]);
            state.Value = (InputState)int.Parse(segments[i++]);
            isActive.Value = segments[i++] == "1";
            RawPosition = parseVector(segments[i++]);
            RawDelta = parseVector(segments[i++]);
            Position = parseVector(segments[i++]);
            Delta = parseVector(segments[i++]);
        }

        void IRecyclable.OnRecycleNew() {}

        void IRecyclable.OnRecycleDestroy()
        {
            state.UnbindAll();
            isActive.UnbindAll();
        }

        void IInput.Release() {}

        void IInput.SetActive(bool active) { }

        void ICursor.SetResolution(Vector2 resolution) { }
    }
}