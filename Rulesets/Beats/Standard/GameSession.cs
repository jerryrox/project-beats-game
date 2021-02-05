using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public class GameSession : Rulesets.GameSession<HitObject> {

        private BeatsStandardProcessor gameProcessor;


        public override GameProcessor GameProcessor => gameProcessor;


        public GameSession(IGraphicObject container) : base(container)
        {
        }

        [InitWithDependency]
        private void Init()
        {
            base.OnHardInit += () =>
            {
                InitGameProcessor();
            };
            base.OnHardDispose += () =>
            {
                gameProcessor = null;
            };
        }

        protected override Rulesets.UI.GameGui CreateGameGui(IGraphicObject container, IDependencyContainer dependencies)
        {
            return container.CreateChild<GameGui>("beats-standard-gui", dependencies: dependencies);
        }

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