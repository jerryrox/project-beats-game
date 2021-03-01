using System.IO;
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

        public void WriteStreamData(BinaryWriter writer)
        {
            writer.Write((int)Key);

            writer.Write((int)state.Value);

            writer.Write(isActive.Value);

            writer.Write(RawPosition.x);
            writer.Write(RawPosition.y);

            writer.Write(RawDelta.x);
            writer.Write(RawDelta.y);

            writer.Write(Position.x);
            writer.Write(Position.y);

            writer.Write(Delta.x);
            writer.Write(Delta.y);
        }

        public void ReadStreamData(BinaryReader reader)
        {
            Key = (KeyCode)reader.ReadInt32();

            state.Value = (InputState)reader.ReadInt32();

            isActive.Value = reader.ReadBoolean();

            RawPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());

            RawDelta = new Vector2(reader.ReadSingle(), reader.ReadSingle());

            Position = new Vector2(reader.ReadSingle(), reader.ReadSingle());

            Delta = new Vector2(reader.ReadSingle(), reader.ReadSingle());
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