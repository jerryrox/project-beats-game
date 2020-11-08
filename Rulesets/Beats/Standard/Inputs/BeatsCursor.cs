using System;
using PBGame.Rulesets.Inputs;
using PBGame.Rulesets.Judgements;
using PBFramework.Data.Bindables;
using PBFramework.Inputs;
using PBFramework.Allocation.Recyclers;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class BeatsCursor : BaseBeatsInput<ICursor>, IRecyclable<BeatsCursor>,
        IInputResultReporter 
    {
        public event Action<JudgementResult> OnResult;

        /// <summary>
        /// The position of the cursor on the hit bar.
        /// </summary>
        public float HitBarPos { get; set; }

        /// <summary>
        /// Whether the cursor is currently holding on the hit bar.
        /// </summary>
        public BindableBool IsOnHitBar { get; private set; } = new BindableBool(false)
        {
            TriggerWhenDifferent = true
        };

        IRecycler<BeatsCursor> IRecyclable<BeatsCursor>.Recycler { get; set; }


        public override void OnRecycleDestroy()
        {
            base.OnRecycleDestroy();
            HitBarPos = 0f;
            IsOnHitBar.Value = false;
        }

        /// <summary>
        /// Makes the cursor emit a judgement result event related to this cursor.
        /// </summary>
        public void ReportNewResult(JudgementResult result)
        {
            OnResult?.Invoke(result);
        }
    }
}