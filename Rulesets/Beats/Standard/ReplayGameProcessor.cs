using System.IO;
using PBGame.IO;
using PBGame.Rulesets.UI.Components;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Beats.Standard.Replays;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public class ReplayGameProcessor : BeatsStandardProcessor
    {
        private ReplayInputter inputter;

        private float lastFrameTime;

        private FileInfo replayFile;
        private DataStreamReader<ReplayFrame> replayReader;
        private StreamReader replayReadStream;

        public override float CurrentTime => lastFrameTime;

        
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
                lastFrameTime = -10000;
                InitReplayReader();
            };
            GameSession.OnSoftDispose += () =>
            {
                DisposeReplayReader();
            };
        }

        public override void JudgePassive(float curTime, HitObjectView hitObjectView)
        {
            // Nothing to do. All judgements should be simulated in Update.
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

                lastFrameTime = frame.Time;

                // Process update for other game modules.
                inputter.UpdateInputs(frame.Time, frame.Inputs);
                HitObjectHolder.UpdateObjects(frame.Time);
                UpdateJudgements(frame);

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
        /// Performs update replay playback.
        /// </summary>
        private void UpdateJudgements(ReplayFrame frame)
        {
            var views = HitObjectHolder.HitObjectViews;
            foreach (var judgement in frame.Judgements)
            {
                // Find the target hit object from path.
                var path = judgement.HitObjectIndexPath;
                BaseHitObjectView hitObjectView = views[path[0]];
                for (int i = 1; i < path.Count; i++)
                {
                    int node = path[i];
                    hitObjectView = hitObjectView.BaseNestedObjects[node];
                }

                if (judgement.IsPassive)
                {
                    foreach (var passiveJudgement in hitObjectView.JudgePassive(frame.Time))
                        AddJudgement(passiveJudgement.Value);
                }
                else
                {
                    var input = frame.Inputs.Find((i) => i.Key == judgement.InputKey);
                    AddJudgement(
                        (hitObjectView as HitObjectView).JudgeInput(frame.Time, input)
                    );
                }
            }

            foreach (var index in frame.DraggersReleased)
            {
                var dragger = views[index] as DraggerView;
                if (dragger != null)
                {
                    dragger.StartCircle.SetHold(false, frame.Time);
                }
            }
            foreach (var index in frame.DraggersHeld)
            {
                var dragger = views[index] as DraggerView;
                if (dragger != null)
                {
                    dragger.StartCircle.SetHold(true, frame.Time);
                }
            }
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