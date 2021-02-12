using System;
using System.IO;
using PBGame.IO;
using PBGame.Stores;
using PBGame.Rulesets.UI.Components;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Beats.Standard.Replays;
using PBGame.Rulesets.Judgements;
using PBFramework.Inputs;
using PBFramework.Dependencies;
using UnityEngine;
using PBGame.Rulesets.Beats.Standard.UI.Components;

namespace PBGame.Rulesets.Beats.Standard
{
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
        private BinaryWriter replayWriteStream;


        public override float CurrentTime => curTime;

        [ReceivesDependency]
        private ITemporaryStore TemporaryStore { get; set; }

        [ReceivesDependency]
        private IRecordStore RecordStore { get; set; }


        [InitWithDependency]
        private void Init()
        {
            replayWriter = new DataStreamWriter<ReplayFrame>(
                ReplayFrameRecycler.GetNext,
                60 * 2,
                writeInterval: 100
            );

            GameSession.OnSoftInit += OnSoftInit;
            GameSession.OnSoftDispose += OnSoftDispose;
            GameSession.OnSkipped += OnSkipped;
        }

        /// <summary>
        /// Records the specified cursor to the replay frame.
        /// </summary>
        public void RecordInput(ICursor cursor)
        {
            if(nextFrame != null && cursor != null)
                nextFrame.AddInput((input) => input.SetFromCursor(cursor));
        }

        public override void RecordDraggerHoldFlag(int draggerIndex, bool isHolding)
        {
            if(nextFrame != null)
                nextFrame.AddDraggerHoldFlag(draggerIndex, isHolding);
        }

        /// <summary>
        /// Records the judgement result for the specified hit object.
        /// </summary>
        public void RecordJudgement(BaseHitObjectView hitObjectView, JudgementResult judgement, bool isPassive, KeyCode keyCode = KeyCode.None)
        {
            if (nextFrame != null && judgement != null && hitObjectView != null && judgement.HitResult != HitResultType.None)
            {
                nextFrame.AddJudgement((j) =>
                {
                    j.SetFromJudgementResult(judgement);
                    j.IsPassive = isPassive;
                    j.InputKey = keyCode;
                    while (hitObjectView != null)
                    {
                        j.HitObjectIndexPath.Insert(0, hitObjectView.ObjectIndex);
                        hitObjectView = hitObjectView.BaseParentView;
                    }
                });
            }
        }

        public override void JudgePassive(float curTime, HitObjectView hitObjectView)
        {
            foreach (var result in hitObjectView.JudgePassive(curTime))
            {
                RecordJudgement(result.Key, result.Value, true);
                AddJudgement(result.Value);
            }
        }

        protected void OnDestroy()
        {
            GameSession.OnSoftInit -= OnSoftInit;
            GameSession.OnSoftDispose -= OnSoftDispose;
            GameSession.OnSkipped -= OnSkipped;
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
                replayWriteStream = new BinaryWriter(replayFile.OpenWrite());
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

            Model.SaveReplay(replayFile);
        }

        /// <summary>
        /// Event called when the game session is soft initializing.
        /// </summary>
        private void OnSoftInit()
        {
            curTime = -10000;
            InitReplayWriter();
        }

        /// <summary>
        /// Event called when the game session is soft disposing.
        /// </summary>
        private void OnSoftDispose()
        {
            DisposeReplayWriter();
            DisposeReplayRecyclers();
        }

        /// <summary>
        /// Event called when the user pressed the skip button.
        /// </summary>
        private void OnSkipped(float time)
        {
            if (nextFrame != null)
                nextFrame.IsSkipped = true;
        }
    }
}