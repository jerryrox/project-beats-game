using System;
using System.Text;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Judgements;
using PBFramework.Allocation.Recyclers;

namespace PBGame.Rulesets.Beats.Standard.Replays
{
    public class ReplayFrame : IStreamableData, IRecyclable<ReplayFrame> {

        private IRecycler<ReplayableInput> replayInputRecycler;
        private IRecycler<ReplayableJudgement> replayJudgementRecycler;


        /// <summary>
        /// The music time at which the frame data was created.
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// The list of inputs that occurred in this frame.
        /// </summary>
        public List<ReplayableInput> Inputs { get; } = new List<ReplayableInput>();

        /// <summary>
        /// The list of hit object index of draggers that were pressed during this frame.
        /// </summary>
        public List<int> DraggersOnHold { get; } = new List<int>();

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
            foreach (var input in Inputs)
                replayInputRecycler.Return(input);
            Inputs.Clear();
            DraggersOnHold.Clear();
            foreach (var judgement in Judgements)
                replayJudgementRecycler.Return(judgement);
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
        /// Adds the specified dragger index to the dragger holding flags list.
        /// Returns the index that was passed as parameter.
        /// </summary>
        public int AddHoldingDragger(int draggerIndex)
        {
            DraggersOnHold.Add(draggerIndex);
            return draggerIndex;
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
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (i > 0)
                    sb.Append('|');
                sb.Append(Inputs[i].ToStreamData());
            }
            sb.Append('/');
            for (int i = 0; i < DraggersOnHold.Count; i++)
            {
                if (i > 0)
                    sb.Append('|');
                sb.Append(DraggersOnHold[i]);
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

            if (splits[1].Length > 0)
            {
                foreach (string inputData in splits[1].Split('|'))
                {
                    var input = replayInputRecycler.GetNext();
                    input.FromStreamData(inputData);
                    Inputs.Add(input);
                }
            }

            if (splits[2].Length > 0)
            {
                foreach (string draggerIndexData in splits[2].Split('|'))
                    DraggersOnHold.Add(int.Parse(draggerIndexData));
            }

            if (splits[3].Length > 0)
            {
                foreach (string judgementData in splits[3].Split('|'))
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