using System;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Rulesets.Inputs;
using PBGame.Rulesets.Judgements;
using PBFramework.Inputs;
using PBFramework.Allocation.Recyclers;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class BeatsKey : BaseBeatsInput<IInput>, IRecyclable<BeatsKey>, 
        IInputResultReporter
    {
        public event Action<JudgementResult> OnResult;

        private BeatsCursor hitCursor;
        private ICursor myCursor;


        /// <summary>
        /// The dragger view instance currently bound to this key.
        /// </summary>
        public DraggerView DraggerView { get; set; }

        IRecycler<BeatsKey> IRecyclable<BeatsKey>.Recycler { get; set; }


        public override void OnRecycleDestroy()
        {
            base.OnRecycleDestroy();
            UnlinkHitCursor();
            DraggerView = null;
        }

        /// <summary>
        /// Associates the specified hit cursor as a link to this key.
        /// </summary>
        public void SetHitCursor(BeatsCursor cursor)
        {
            UnlinkHitCursor();

            hitCursor = cursor;
            if (hitCursor != null)
            {
                hitCursor.OnResult += OnHitCursorResult;
                hitCursor.OnRelease += OnHitCursorRelease;
            }
        }

        /// <summary>
        /// Sets the cursor representation of inner input.
        /// </summary>
        public void SetInputAsCursor(ICursor cursor) => this.myCursor = cursor;

        /// <summary>
        /// Returns the base key input instance as ICursor, if applicable.
        /// </summary>
        public ICursor GetInputAsCursor() => myCursor;

        /// <summary>
        /// Unlinks events attached to the hit cursor and removes the reference to it.
        /// </summary>
        private void UnlinkHitCursor()
        {
            if (hitCursor != null)
            {
                hitCursor.OnResult -= OnHitCursorResult;
                hitCursor.OnRelease -= OnHitCursorRelease;
            }
            hitCursor = null;
        }

        /// <summary>
        /// Event called when the hit cursor has been released.
        /// </summary>
        private void OnHitCursorRelease() => UnlinkHitCursor();

        /// <summary>
        /// Event called when the linked hit cursor has a new judgement result emitted.
        /// </summary>
        private void OnHitCursorResult(JudgementResult result) => OnResult?.Invoke(result);
    }
}