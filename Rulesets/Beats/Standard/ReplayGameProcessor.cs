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
        private bool didSkip;

        private FileInfo replayFile;
        private DataStreamReader<ReplayFrame> replayReader;
        private BinaryReader replayReadStream;

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

            GameSession.OnSoftInit += OnSoftInit;
            GameSession.OnSoftDispose += OnSoftDispose;
        }

        public override void JudgePassive(float curTime, HitObjectView hitObjectView)
        {
            // Nothing to do. All judgements should be simulated in Update.
        }

        protected void OnDestroy()
        {
            GameSession.OnSoftInit -= OnSoftInit;
            GameSession.OnSoftDispose -= OnSoftDispose;
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
                {
                    if (didSkip)
                        lastFrameTime = musicTime;
                    break;
                }

                lastFrameTime = frame.Time;
                didSkip = frame.IsSkipped;

                // Process update for other game modules.
                inputter.UpdateInputs(frame.Time, frame.Inputs);
                HitObjectHolder.UpdateObjects(musicTime);
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

            foreach (var pair in frame.DraggerHoldFlags)
            {
                var dragger = views[pair.Key] as DraggerView;
                if (dragger != null)
                {
                    dragger.StartCircle.SetHold(pair.Value, frame.Time);
                }
            }
            
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

                AddJudgement(
                    hitObjectView.SetResult(judgement.HitResult, judgement.HitOffset)
                );
            }
        }

        /// <summary>
        /// Initializes a replay reader instance for replay playback.
        /// </summary>
        private void InitReplayReader()
        {
            replayReadStream = new BinaryReader(replayFile.OpenRead());
            replayReader.StartStream(replayReadStream);
        }

        /// <summary>
        /// Disposes the current replay writer instance.
        /// </summary>
        private void DisposeReplayReader()
        {
            replayReader.StopStream();
            replayReadStream.Dispose();
            replayReadStream = null;
        }

        /// <summary>
        /// Event called when the game session is soft initializing.
        /// </summary>
        private void OnSoftInit()
        {
            lastFrameTime = -10000;
            InitReplayReader();
        }

        /// <summary>
        /// Event called when the game session is soft disposing.
        /// </summary>
        private void OnSoftDispose()
        {
            DisposeReplayReader();
            DisposeReplayRecyclers();
        }
    }
}