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
                // TODO: Determine whether it's a player-controlled session.
                var playArea = Dependencies.Get<PlayAreaContainer>();
                var hitObjectHolder = Dependencies.Get<HitObjectHolder>();

                // Initialize inputter
                var localPlayerInputter = new LocalPlayerInputter(playArea.HitBar, hitObjectHolder);
                Dependencies.Inject(localPlayerInputter);
                gameInputter = localPlayerInputter;

                // Pass inputter to hit object holder
                hitObjectHolder.SetInputter(gameInputter);
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
    }
}