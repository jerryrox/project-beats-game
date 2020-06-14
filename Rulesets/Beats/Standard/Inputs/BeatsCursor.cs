using PBFramework.Data.Bindables;
using PBFramework.Inputs;
using PBFramework.Allocation.Recyclers;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class BeatsCursor : BaseBeatsInput<ICursor>, IRecyclable<BeatsCursor> {

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
    }
}