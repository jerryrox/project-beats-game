using System;
using System.IO;
using PBGame.UI;
using PBGame.IO;
using PBGame.Stores;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBFramework.Inputs;
using PBFramework.Dependencies;
using UnityEngine.EventSystems;

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
            float curTime = hitObjectHolder.CurrentTime;

            UpdateInputs(curTime);

            if (!GameSession.IsPaused)
            {
                var cursors = InputManager.GetCursors();
                foreach (var cursor in cursors)
                {
                    if (cursor.State.Value == InputState.Press)
                    {
                        // If hit on the hit bar, register this as a new BeatsCursor.
                        if (!hitBarCursor.IsActive && IsOnHitBar(cursor, out float pos))
                            TriggerCursorPress(cursor, pos);
                        // If not hit on hit bar, this is treated as a key stoke.
                        else
                            TriggerKeyPress(cursor, cursor);
                    }

                    // Record replay input data
                    if (replayInputWriter != null)
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
        }

        protected override void OnHardDispose()
        {
            base.OnHardDispose();

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

        /// <summary>
        /// Initializes the replay input writer instance.
        /// </summary>
        private void InitReplayInputWriter()
        {
            // TODO: Handle only if saving replay option is enabled.
            // {
            //     return;
            // }

            replayInputWriter = new DataStreamWriter<ReplayableInput>(InputManager.MaxTouchCount * 60, writeInterval: 500);
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