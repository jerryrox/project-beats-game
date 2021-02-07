using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Beats.Standard.Replays;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public class BeatsStandardSession : GameSession<HitObject> {

        private BeatsStandardProcessor gameProcessor;

        private ManagedRecycler<ReplayableInput> replayInputRecycler;
        private ManagedRecycler<ReplayFrame> replayFrameRecycler;


        public override GameProcessor GameProcessor => gameProcessor;


        public BeatsStandardSession(IGraphicObject container) : base(container)
        {
            replayInputRecycler = new ManagedRecycler<ReplayableInput>(CreateReplayInput);
            replayFrameRecycler = new ManagedRecycler<ReplayFrame>(CreateReplayFrame);
        }

        [InitWithDependency]
        private void Init()
        {
            Dependencies.CacheAs<IRecycler<ReplayableInput>>(replayInputRecycler);
            Dependencies.CacheAs<IRecycler<ReplayFrame>>(replayFrameRecycler);

            base.OnHardInit += () =>
            {
                InitGameProcessor();
            };
            base.OnHardDispose += () =>
            {
                replayInputRecycler.ReturnAll();
                replayFrameRecycler.ReturnAll();

                gameProcessor.Destroy();
                gameProcessor = null;
            };
        }

        protected override Rulesets.UI.GameGui CreateGameGui(IGraphicObject container, IDependencyContainer dependencies)
        {
            return container.CreateChild<GameGui>("beats-standard-gui", dependencies: dependencies);
        }

        /// <summary>
        /// Creates a new replayable input instance for recycler.
        /// </summary>
        private ReplayableInput CreateReplayInput() => new ReplayableInput();

        /// <summary>
        /// Creates a new replay frame instance for recycler.
        /// </summary>
        private ReplayFrame CreateReplayFrame() => new ReplayFrame(replayInputRecycler);

        /// <summary>
        /// Initializes the game processor instance.
        /// </summary>
        private void InitGameProcessor()
        {
            if (CurrentParameter.IsReplay)
                gameProcessor = GameGui.CreateChild<ReplayGameProcessor>();
            else
                gameProcessor = GameGui.CreateChild<LocalGameProcessor>();

            Dependencies.Cache(gameProcessor);
        }
    }
}