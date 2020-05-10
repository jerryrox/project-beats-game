using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Beats.Standard.Scoring;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public class GameSession : Rulesets.GameSession<HitObject> {
    
        public GameSession(IGraphicObject container) : base(container)
        {
        }

        protected override Rulesets.UI.GameGui CreateGameGui(IGraphicObject container, IDependencyContainer dependencies)
        {
            return container.CreateChild<GameGui>("beats-standard-gui", dependencies: dependencies);
        }

        protected override Rulesets.Scoring.IScoreProcessor CreateScoreProcessor() => new ScoreProcessor();
    }
}