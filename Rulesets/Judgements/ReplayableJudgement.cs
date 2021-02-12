using System.IO;
using System.Collections.Generic;
using PBGame.IO;
using PBFramework.Allocation.Recyclers;
using UnityEngine;

namespace PBGame.Rulesets.Judgements
{
    public class ReplayableJudgement : IStreamableData, IRecyclable<ReplayableJudgement>
    {
        /// <summary>
        /// The result evaluated from the judgement.
        /// </summary>
        public HitResultType HitResult { get; set; }

        /// <summary>
        /// The offset between the actual hit time and the hit object's time.
        /// </summary>
        public float HitOffset { get; set; }

        /// <summary>
        /// Whether the judgement was made passively or via input.
        /// </summary>
        public bool IsPassive { get; set; }

        /// <summary>
        /// If not passive judgement, the keycode of the input that triggered this judgement.
        /// </summary>
        public KeyCode InputKey { get; set; }

        /// <summary>
        /// A path which describes the index of a hit object at any nested depth.
        /// The lowest index represents the root hit object.
        /// </summary>
        public List<int> HitObjectIndexPath { get; } = new List<int>();

        IRecycler<ReplayableJudgement> IRecyclable<ReplayableJudgement>.Recycler { get; set; }


        /// <summary>
        /// Resets the state of the judgement.
        /// </summary>
        public void Reset()
        {
            HitResult = HitResultType.None;
            HitOffset = 0;
            IsPassive = false;
            InputKey = KeyCode.None;
            HitObjectIndexPath.Clear();
        }

        /// <summary>
        /// Sets the attributes based on the specified judgement result.
        /// </summary>
        public void SetFromJudgementResult(JudgementResult result)
        {
            HitResult = result.HitResult;
            HitOffset = result.HitOffset;
        }

        public void WriteStreamData(BinaryWriter writer)
        {
            writer.Write((int)HitResult);

            writer.Write(HitOffset);

            writer.Write(IsPassive);

            writer.Write((int)InputKey);

            writer.Write(HitObjectIndexPath.Count);
            for (int i = 0; i < HitObjectIndexPath.Count; i++)
                writer.Write(HitObjectIndexPath[i]);
        }

        public void ReadStreamData(BinaryReader reader)
        {
            HitResult = (HitResultType)reader.ReadInt32();

            HitOffset = reader.ReadSingle();

            IsPassive = reader.ReadBoolean();

            InputKey = (KeyCode)reader.ReadInt32();

            int indexPathCount = reader.ReadInt32();
            for (int i = 0; i < indexPathCount; i++)
                HitObjectIndexPath.Add(reader.ReadInt32());
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