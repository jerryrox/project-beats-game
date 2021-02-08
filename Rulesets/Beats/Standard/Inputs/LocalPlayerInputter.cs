using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBFramework.Inputs;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class LocalPlayerInputter : BaseInputter
    {
        private LocalGameProcessor gameProcessor;

        [ReceivesDependency]
        private IInputManager InputManager { get; set; }


        public LocalPlayerInputter(HitBarDisplay hitBar, HitObjectHolder hitObjectHolder) : base(hitBar, hitObjectHolder)
        { }

        /// <summary>
        /// Sets the game processor that manages this inputter.
        /// </summary>
        public void SetGameProcessor(LocalGameProcessor gameProcessor)
        {
            this.gameProcessor = gameProcessor;
        }

        public void UpdateInputs(float curTime)
        {
            if (!GameSession.IsPaused)
            {
                // Process beats cursor aiming.
                if (hitBarCursor.IsActive)
                {
                    // Aiming on hit bar?
                    if (IsOnHitBar(hitBarCursor.Input, out float pos))
                    {
                        hitBarCursor.HitBarPos = pos;
                        hitBarCursor.IsOnHitBar.Value = true;
                    }
                    else
                    {
                        hitBarCursor.IsOnHitBar.Value = false;
                    }

                    // Check all key strokes whether the cursor is within the hit object boundary.
                    foreach (var key in keyRecycler.ActiveObjects)
                    {
                        key.LastUpdateTime = curTime;
                        if (key.IsActive && key.DraggerView != null)
                        {
                            var dragger = key.DraggerView;
                            bool isHolding = dragger.IsCursorInRange(pos);
                            dragger.StartCircle.SetHold(isHolding, curTime);
                        }
                    }
                }

                var cursors = InputManager.GetCursors();
                foreach (var cursor in cursors)
                {
                    if (cursor.State.Value == InputState.Press)
                    {
                        BeatsKey pressedKey = null;
                        // If hit on the hit bar, register this as a new BeatsCursor.
                        if (!hitBarCursor.IsActive && IsOnHitBar(cursor, out float pos))
                            pressedKey = TriggerCursorPress(curTime, cursor, pos);
                        // If not hit on hit bar, this is treated as a key stoke.
                        else
                            pressedKey = TriggerKeyPress(curTime, cursor, cursor);
                        JudgeKeyPress(curTime, pressedKey);
                    }

                    // Record replay input data
                    if (cursor.IsActive.Value)
                        gameProcessor.RecordInput(cursor);
                }
            }
        }

        protected override void OnKeyStateRelease(BeatsKey key)
        {
            InvokeKeyRelease(key);
            JudgeKeyRelease(key);
            
            keyRecycler.Return(key);
        }

        /// <summary>
        /// Judges the specified key press against hit objects.
        /// </summary>
        private void JudgeKeyPress(float time, BeatsKey key)
        {
            // Return if no cursor is active.
            if(!hitBarCursor.IsActive)
                return;

            // Find the first hit object where the cursor is within the X range.
            foreach (var objView in hitObjectHolder.GetActiveObjects())
            {
                if (objView.IsCursorInRange(hitBarCursor.HitBarPos))
                {
                    // Associate the hit object view with the key stroke.
                    if(objView is DraggerView draggerView)
                        key.DraggerView = draggerView;
                    var judgement = objView.JudgeInput(time, key.Input);
                    gameProcessor.RecordJudgement(objView, judgement, false, keyCode: key.Input.Key);
                    gameProcessor.AddJudgement(judgement);
                    break;
                }
            }
        }

        /// <summary>
        /// Judges the specified key release against hit objects.
        /// </summary>
        private void JudgeKeyRelease(BeatsKey key)
        {
            if (key.DraggerView == null)
                return;

            key.DraggerView.StartCircle.SetHold(false, key.LastUpdateTime);
        }
    }
}