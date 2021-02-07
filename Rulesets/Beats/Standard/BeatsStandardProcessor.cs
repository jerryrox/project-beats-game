using PBGame.UI.Models;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Beats.Standard.Replays;
using PBGame.Rulesets.Judgements;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public abstract class BeatsStandardProcessor : GameProcessor
    {
        private IGameInputter inputter;


        [ReceivesDependency]
        protected HitObjectHolder HitObjectHolder { get; set; }

        [ReceivesDependency]
        protected PlayAreaContainer PlayAreaContainer { get; set; }

        [ReceivesDependency]
        protected TouchEffectDisplay TouchEffectDisplay { get; set; }

        [ReceivesDependency]
        protected IRecycler<ReplayFrame> ReplayFrameRecycler { get; set; }

        [ReceivesDependency]
        protected IRecycler<ReplayableJudgement> ReplayJudgementRecycler { get; set; }

        [ReceivesDependency]
        protected IRecycler<ReplayableInput> ReplayInputRecycler { get; set; }

        [ReceivesDependency]
        protected GameModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            // Init inputter
            inputter = CreateGameInputter();
            TouchEffectDisplay.SetInputter(inputter);

            // Assign processor instance to other game modules.
            HitObjectHolder.SetGameProcessor(this);
        }

        /// <summary>
        /// Handles passive judgement of the specified hit object view.
        /// </summary>
        public abstract void JudgePassive(float curTime, HitObjectView hitObjectView);

        protected abstract void Update();

        /// <summary>
        /// Adds the specified judgement result to the score processor.
        /// </summary>
        public void AddJudgement(JudgementResult result)
        {
            if (result != null)
            {
                if(inputter.HitBarCursor.IsActive)
                    inputter.HitBarCursor.ReportNewResult(result);

                GameSession?.ScoreProcessor.ProcessJudgement(result);
            }
        }

        /// <summary>
        /// Creates a new game inputter instance.
        /// </summary>
        protected abstract IGameInputter CreateGameInputter();

        /// <summary>
        /// Cleans up the replay-related object recyclers.
        /// </summary>
        protected void DisposeReplayRecyclers()
        {
            (ReplayFrameRecycler as ManagedRecycler<ReplayFrame>)?.ReturnAll();
            (ReplayJudgementRecycler as ManagedRecycler<ReplayableJudgement>)?.ReturnAll();
            (ReplayInputRecycler as ManagedRecycler<ReplayableInput>)?.ReturnAll();
        }
    }
}