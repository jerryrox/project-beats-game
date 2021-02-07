using System.Text;
using System.Collections.Generic;
using PBGame.IO;
using PBFramework.Allocation.Recyclers;

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

        public string ToStreamData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((int)HitResult);
            sb.Append(';');
            sb.Append(HitOffset);
            sb.Append(';');
            for (int i = 0; i < HitObjectIndexPath.Count; i++)
            {
                if (i > 0)
                    sb.Append(',');
                sb.Append(HitObjectIndexPath[i]);
            }
            return sb.ToString();
        }

        public void FromStreamData(string data)
        {
            string[] splits = data.Split(';');

            HitResult = (HitResultType)int.Parse(splits[0]);

            HitOffset = float.Parse(splits[1]);

            if (splits[2].Length > 2)
            {
                foreach (string indexData in splits[2].Split(','))
                    HitObjectIndexPath.Add(int.Parse(indexData));
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