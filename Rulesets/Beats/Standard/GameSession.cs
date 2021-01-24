using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public class GameSession : Rulesets.GameSession<HitObject> {

        // TODO: Support for different types of game inputter.
        private IGameInputter gameInputter;


        public GameSession(IGraphicObject container) : base(container)
        {
        }

        [InitWithDependency]
        private void Init()
        {
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
            var playArea = Dependencies.Get<PlayAreaContainer>();
            var hitObjectHolder = Dependencies.Get<HitObjectHolder>();
            var touchEffectDisplay = Dependencies.Get<TouchEffectDisplay>();

            // TODO: Determine whether it's a player-controlled session.
            // Initialize inputter
            var localPlayerInputter = new LocalPlayerInputter(playArea.HitBar, hitObjectHolder);
            Dependencies.Inject(localPlayerInputter);
            gameInputter = localPlayerInputter;

            // Pass inputter to dependencies.
            hitObjectHolder.SetInputter(gameInputter);
            touchEffectDisplay.SetInputter(gameInputter);
        }
    }
}