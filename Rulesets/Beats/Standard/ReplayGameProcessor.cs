using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Beats.Standard.Replays;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard
{
    public class ReplayGameProcessor : BeatsStandardProcessor
    {
        private ReplayInputter inputter;

        private ReplayFrame curFrame;

        private FileInfo replayFile;
        private DataStreamReader<ReplayFrame> replayReader;
        private StreamReader replayReadStream;

        public override float CurrentTime => curFrame?.Time ?? -10000;

        
        [InitWithDependency]
        private void Init(IRecycler<ReplayFrame> replayFrameRecycler)
        {
            replayFile = GameSession.CurrentParameter.ReplayFile;

            replayReader = new DataStreamReader<ReplayFrame>(
                replayFrameRecycler.GetNext,
                60 * 5,
                readInterval: 100
            );

            GameSession.OnSoftInit += () =>
            {
                curFrame = null;
                InitReplayReader();
            };
            GameSession.OnSoftDispose += () =>
            {
                DisposeReplayReader();
            };
        }

        public override void JudgePassive(float curTime, HitObjectView hitObjectView)
        {
            // TODO:
            // throw new NotImplementedException();
        }

        protected override void Update()
        {
            if (!GameSession.IsPlaying)
                return;

            float musicTime = MusicController.CurrentTime;
            while (true)
            {
                var frame = replayReader.PeekData();
                if (frame == null || frame.Time > musicTime)
                    break;

                curFrame = frame;

                // Process update for other game modules.
                inputter.UpdateInputs(frame.Time, frame.Inputs);
                HitObjectHolder.UpdateObjects(frame.Time);

                // TODO: Playback judgements.

                replayReader.AdvanceIndex();
            }
        }

        protected override IGameInputter CreateGameInputter()
        {
            inputter = new ReplayInputter(
                GameParameter.ReplayFile,
                PlayAreaContainer.HitBar,
                HitObjectHolder
            );
            Dependencies.Inject(inputter);
            return inputter;
        }

        /// <summary>
        /// Initializes a replay reader instance for replay playback.
        /// </summary>
        private void InitReplayReader()
        {
            replayReadStream = new StreamReader(replayFile.OpenRead());
            replayReader.StartStream(replayReadStream);
        }

        /// <summary>
        /// Disposes the current replay writer instance.
        /// </summary>
        private void DisposeReplayReader()
        {
            replayReader.StopStream();
            replayReadStream = null;
        }
    }
}