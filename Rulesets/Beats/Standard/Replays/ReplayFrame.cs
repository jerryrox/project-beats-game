using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Judgements;
using PBFramework.Allocation.Recyclers;

namespace PBGame.Rulesets.Beats.Standard.Replays
{
    public class ReplayFrame : IStreamableData, IRecyclable<ReplayFrame>
    {
        private IRecycler<ReplayableInput> replayInputRecycler;
        private IRecycler<ReplayableJudgement> replayJudgementRecycler;


        /// <summary>
        /// The music time at which the frame data was created.
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// Whether the user has skipped from this frame.
        /// </summary>
        public bool IsSkipped { get; set; }

        /// <summary>
        /// The list of inputs that occurred in this frame.
        /// </summary>
        public List<ReplayableInput> Inputs { get; } = new List<ReplayableInput>();

        /// <summary>
        /// The list of dragger object index mapped to a flag indicating whether the dragger was pressed or released on this frame.
        /// </summary>
        public List<KeyValuePair<int, bool>> DraggerHoldFlags = new List<KeyValuePair<int, bool>>();

        /// <summary>
        /// The list of judgements that occurred in this frame.
        /// </summary>
        public List<ReplayableJudgement> Judgements { get; } = new List<ReplayableJudgement>();

        IRecycler<ReplayFrame> IRecyclable<ReplayFrame>.Recycler { get; set; }


        public ReplayFrame(IRecycler<ReplayableInput> replayInputRecycler, IRecycler<ReplayableJudgement> replayJudgementRecycler)
        {
            this.replayInputRecycler = replayInputRecycler;
            this.replayJudgementRecycler = replayJudgementRecycler;
        }

        /// <summary>
        /// Clears the state of the frame.
        /// </summary>
        public void Reset()
        {
            Time = 0;
            IsSkipped = false;
            Inputs.Clear();
            DraggerHoldFlags.Clear();
            Judgements.Clear();
        }

        /// <summary>
        /// Adds a new replayable input data to the frame and returns it.
        /// </summary>
        public ReplayableInput AddInput(Action<ReplayableInput> initializer)
        {
            var input = replayInputRecycler.GetNext();
            initializer.Invoke(input);
            Inputs.Add(input);
            return input;
        }

        /// <summary>
        /// Adds the specified holding/released flag for the dragger object index.
        /// </summary>
        public void AddDraggerHoldFlag(int draggerIndex, bool isHeld)
        {
            DraggerHoldFlags.Add(new KeyValuePair<int, bool>(draggerIndex, isHeld));
        }

        /// <summary>
        /// Adds a new replayable judgement result data to the frame and returns it.
        /// </summary>
        public ReplayableJudgement AddJudgement(Action<ReplayableJudgement> initializer)
        {
            var judgement = replayJudgementRecycler.GetNext();
            initializer.Invoke(judgement);
            Judgements.Add(judgement);
            return judgement;
        }

        public void WriteStreamData(BinaryWriter writer)
        {
            writer.Write(Time);

            writer.Write(IsSkipped);

            writer.Write(Inputs.Count);
            for (int i = 0; i < Inputs.Count; i++)
                Inputs[i].WriteStreamData(writer);

            writer.Write(DraggerHoldFlags.Count);
            for (int i = 0; i < DraggerHoldFlags.Count; i++)
            {
                writer.Write(DraggerHoldFlags[i].Key);
                writer.Write(DraggerHoldFlags[i].Value);
            }

            writer.Write(Judgements.Count);
            for (int i = 0; i < Judgements.Count; i++)
                Judgements[i].WriteStreamData(writer);
        }

        public void ReadStreamData(BinaryReader reader)
        {
            Reset();

            Time = reader.ReadSingle();

            IsSkipped = reader.ReadBoolean();

            int inputCount = reader.ReadInt32();
            for (int i = 0; i < inputCount; i++)
            {
                var input = replayInputRecycler.GetNext();
                input.ReadStreamData(reader);
                Inputs.Add(input);
            }

            int holdFlagCount = reader.ReadInt32();
            for (int i = 0; i < holdFlagCount; i++)
            {
                DraggerHoldFlags.Add(new KeyValuePair<int, bool>(reader.ReadInt32(), reader.ReadBoolean()));
            }

            int judgementCount = reader.ReadInt32();
            for (int i = 0; i < judgementCount; i++)
            {
                var judgement = replayJudgementRecycler.GetNext();
                judgement.ReadStreamData(reader);
                Judgements.Add(judgement);
            }
        }

        void IRecyclable.OnRecycleNew()
        {
            Reset();
        }

        void IRecyclable.OnRecycleDestroy()
        {
            Reset();
        }
    }
}