using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Stores;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Beats.Standard.Replays;
using PBFramework.UI;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;

namespace PBGame.Rulesets.Beats.Standard
{
    // TODO: Record judgements.
    public class LocalGameProcessor : BeatsStandardProcessor
    {
        private LocalPlayerInputter inputter;

        /// <summary>
        /// The next container for recording frame data.
        /// </summary>
        private ReplayFrame nextFrame;

        private float curTime;

        private FileInfo replayFile;
        private DataStreamWriter<ReplayFrame> replayWriter;
        private StreamWriter replayWriteStream;


        public override float CurrentTime => curTime;

        [ReceivesDependency]
        private ITemporaryStore TemporaryStore { get; set; }

        [ReceivesDependency]
        private IRecordStore RecordStore { get; set; }


        [InitWithDependency]
        private void Init(IRecycler<ReplayFrame> replayFrameRecycler)
        {
            replayWriter = new DataStreamWriter<ReplayFrame>(
                replayFrameRecycler.GetNext,
                60 * 2,
                writeInterval: 100
            );

            GameSession.OnSoftInit += () =>
            {
                curTime = -10000;

                InitReplayWriter();
            };
            GameSession.OnSoftDispose += () =>
            {
                DisposeReplayWriter();
            };
        }

        /// <summary>
        /// Records the specified cursor to the replay frame.
        /// </summary>
        public void RecordInput(ICursor cursor)
        {
            if(nextFrame != null)
                nextFrame.AddInput((input) => input.SetFromCursor(cursor));
        }

        /// <summary>
        /// Records the dragging flag for the specified dragger index.
        /// </summary>
        public void RecordHoldingDragger(int draggerIndex)
        {
            if(nextFrame != null)
                nextFrame.AddHoldingDragger(draggerIndex);
        }

        public override void JudgePassive(float curTime, HitObjectView hitObjectView)
        {
            foreach (var judgement in hitObjectView.JudgePassive(curTime))
                AddJudgement(judgement);
        }

        protected override void Update()
        {
            if (!GameSession.IsPlaying)
                return;

            curTime = MusicController.CurrentTime;

            // Allocate next frame data
            nextFrame = null;
            if (replayWriter != null)
            {
                nextFrame = replayWriter.NextWriteItem;
                nextFrame?.Reset();
            }

            // Process update for other game modules.
            inputter.UpdateInputs(curTime);
            HitObjectHolder.UpdateObjects(curTime);

            // Record replay frame
            if (nextFrame != null)
            {
                nextFrame.Time = curTime;
                replayWriter.WriteData(nextFrame);
            }
        }

        protected override IGameInputter CreateGameInputter()
        {
            inputter = new LocalPlayerInputter(
                PlayAreaContainer.HitBar,
                HitObjectHolder
            );
            inputter.SetGameProcessor(this);
            Dependencies.Inject(inputter);
            return inputter;
        }

        /// <summary>
        /// Initializes a replay writer instance for replay recording.
        /// </summary>
        private void InitReplayWriter()
        {
            // TODO: Check whether replay save option is enabled.

            if (replayWriter != null)
            {
                replayFile = TemporaryStore.GetReplayDataFile(Guid.NewGuid().ToString());
                replayWriteStream = new StreamWriter(replayFile.OpenWrite());
                replayWriter.StartStream(replayWriteStream);
            }
        }

        /// <summary>
        /// Disposes the current replay writer instance.
        /// </summary>
        private void DisposeReplayWriter()
        {
            if (replayWriter != null)
                replayWriter.StopStream();
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
    }
}