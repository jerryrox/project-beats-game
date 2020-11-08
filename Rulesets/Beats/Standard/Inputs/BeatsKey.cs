using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBFramework.Inputs;
using PBFramework.Allocation.Recyclers;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class BeatsKey : BaseBeatsInput<IInput>, IRecyclable<BeatsKey> {

        private ICursor cursor;


        /// <summary>
        /// The dragger view instance currently bound to this key.
        /// </summary>
        public DraggerView DraggerView { get; set; }

        IRecycler<BeatsKey> IRecyclable<BeatsKey>.Recycler { get; set; }


        public override void OnRecycleDestroy()
        {
            base.OnRecycleDestroy();
            DraggerView = null;
        }

        /// <summary>
        /// Sets the cursor representation of inner input.
        /// </summary>
        public void SetInputAsCursor(ICursor cursor) => this.cursor = cursor;

        /// <summary>
        /// Returns the base key input instance as ICursor, if applicable.
        /// </summary>
        public ICursor GetInputAsCursor() => cursor;
    }
}