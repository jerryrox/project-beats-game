using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public class GameSession : Rulesets.GameSession<HitObject> {

        private GameModuleProvider moduleProvider;

        // TODO: Support for different types of game inputter.
        private IGameInputter gameInputter;


        public GameSession(IGraphicObject container) : base(container)
        {
        }

        [InitWithDependency]
        private void Init()
        {
            moduleProvider = new GameModuleProvider(Dependencies);
            Dependencies.Cache(moduleProvider);

            base.OnHardInit += () =>
            {
                InitInputter();
            };
            base.OnHardDispose += () =>
            {
                gameInputter = null;
            };
        }

        protected override Rulesets.UI.GameGui CreateGameGui(IGraphicObject container, IDependencyContainer dependencies)
        {
            return container.CreateChild<GameGui>("beats-standard-gui", dependencies: dependencies);
        }

        /// <summary>
        /// Initializes inputter for gameplay.
        /// </summary>
        private void InitInputter()
        {
            // Initialize inputter
            var inputter = moduleProvider.GetGameInputter();

            // Pass inputter to dependencies.
            var hitObjectHolder = Dependencies.Get<HitObjectHolder>();
            var touchEffectDisplay = Dependencies.Get<TouchEffectDisplay>();
            hitObjectHolder.SetInputter(gameInputter);
            touchEffectDisplay.SetInputter(gameInputter);
        }
    }
}