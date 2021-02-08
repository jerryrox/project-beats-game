using System;
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

        public string ToStreamData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Time);
            sb.Append('/');
            sb.Append(IsSkipped ? "1" : "0");
            sb.Append('/');
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (i > 0)
                    sb.Append('|');
                sb.Append(Inputs[i].ToStreamData());
            }
            sb.Append('/');
            for (int i = 0; i < DraggerHoldFlags.Count; i++)
            {
                if (i > 0)
                    sb.Append('|');
                sb.Append(DraggerHoldFlags[i].Key).Append(",").Append(DraggerHoldFlags[i].Value ? "1" : "0");
            }
            sb.Append('/');
            for (int i = 0; i < Judgements.Count; i++)
            {
                if (i > 0)
                    sb.Append('|');
                sb.Append(Judgements[i].ToStreamData());
            }
            return sb.ToString();
        }

        public void FromStreamData(string data)
        {
            Reset();
            
            string[] splits = data.Split('/');

            Time = float.Parse(splits[0]);

            IsSkipped = splits[1] == "1";

            if (splits[2].Length > 0)
            {
                foreach (string inputData in splits[2].Split('|'))
                {
                    var input = replayInputRecycler.GetNext();
                    input.FromStreamData(inputData);
                    Inputs.Add(input);
                }
            }

            if (splits[3].Length > 0)
            {
                foreach (string draggerHoldData in splits[3].Split('|'))
                {
                    string[] pair = draggerHoldData.Split(',');
                    DraggerHoldFlags.Add(new KeyValuePair<int, bool>(int.Parse(pair[0]), pair[1] == "1"));
                }
            }

            if (splits[4].Length > 0)
            {
                foreach (string judgementData in splits[4].Split('|'))
                {
                    var judgement = replayJudgementRecycler.GetNext();
                    judgement.FromStreamData(judgementData);
                    Judgements.Add(judgement);
                }
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