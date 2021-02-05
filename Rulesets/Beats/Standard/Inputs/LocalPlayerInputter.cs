using System;
using System.IO;
using PBGame.UI;
using PBGame.IO;
using PBGame.Stores;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBFramework.Inputs;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class LocalPlayerInputter : BaseInputter, IInputReceiver
    {
        private DataStreamWriter<ReplayableInput> replayInputWriter;
        private StreamWriter replayWriteStream;

        public int InputLayer => InputLayers.GameScreenComponents;

        [ReceivesDependency]
        private IInputManager InputManager { get; set; }

        [ReceivesDependency]
        private ITemporaryStore TemporaryStore { get; set; }

        [ReceivesDependency]
        private IRecordStore RecordStore { get; set; }


        public LocalPlayerInputter(HitBarDisplay hitBar, HitObjectHolder hitObjectHolder) : base(hitBar, hitObjectHolder)
        { }

        [InitWithDependency]
        private void Init()
        {
            InitReplayInputWriter();
        }

        public bool ProcessInput()
        {
            float curTime = gameProcessor.CurrentTime;

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
                            key.DraggerView.StartCircle.SetHold(key.DraggerView.IsCursorInRange(pos), curTime);
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
                    if (replayInputWriter != null && cursor.IsActive.Value)
                        RecordInputData(curTime, cursor);
                }
            }
            return true;
        }

        int IComparable<IInputReceiver>.CompareTo(IInputReceiver other) => other.InputLayer.CompareTo(InputLayer);

        void IInputReceiver.PrepareInputSort() { }

        protected override void OnSoftInit()
        {
            base.OnSoftInit();

            InputManager.AddReceiver(this);

            // Setup input writer
            if (replayInputWriter != null)
            {
                replayFile = TemporaryStore.GetReplayDataFile(Guid.NewGuid().ToString());
                replayWriteStream = new StreamWriter(replayFile.OpenWrite());
                replayInputWriter.StartStream(replayWriteStream);
            }
        }

        protected override void OnSoftDispose()
        {
            base.OnSoftDispose();

            InputManager.RemoveReceiver(this);

            if (replayInputWriter != null)
                replayInputWriter.StopStream();
            if (replayWriteStream != null)
                replayWriteStream.Dispose();
            replayWriteStream = null;

            if (replayFile != null)
            {
                var lastRecord = Model.LastRecord;
                if (lastRecord != null && lastRecord.IsClear)
                {
                    var replayFileDest = RecordStore.GetReplayFile(lastRecord);
                    replayFile.MoveTo(replayFileDest.FullName);
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
        /// Initializes the replay input writer instance.
        /// </summary>
        private void InitReplayInputWriter()
        {
            // TODO: Handle only if saving replay option is enabled.
            // {
            //     return;
            // }

            replayInputWriter = new DataStreamWriter<ReplayableInput>(InputManager.MaxCursorCount * 60, writeInterval: 500);
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
                    gameProcessor.AddJudgement(objView.JudgeInput(time, key.Input));
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

        /// <summary>
        /// Records the specified input for replay feature.
        /// </summary>
        private void RecordInputData(float curTime, ICursor cursor)
        {
            var nextData = replayInputWriter.NextWriteItem;
            nextData.SetFromCursor(curTime, cursor);
            replayInputWriter.WriteData(nextData);
        }
    }
}