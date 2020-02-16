using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Beats.Standard.Maps;
using PBGame.Rulesets.Beats.Standard.Difficulty;
using PBGame.Rulesets.Beats.Standard.Judgements;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public class ModeService : Rulesets.ModeService {

        public override string Name => "Beats Standard";

        public override GameModes GameMode => GameModes.BeatsStandard;


        public ModeService(IDependencyContainer dependencies) : base(dependencies)
        {
        }

        public override Rulesets.Maps.IMapConverter CreateConverter(IMap map) => new MapConverter(map);

        public override Rulesets.Maps.IMapProcessor CreateProcessor(IMap map) => new Standard.Maps.MapProcessor(map);

        public override Rulesets.Difficulty.IDifficultyCalculator CreateDifficultyCalculator(IMap map) => new DifficultyCalculator(map);

        public override Rulesets.Judgements.HitTiming CreateTiming() => new HitTiming();

        protected override IGameSession CreateSession(IGraphicObject container) => new GameSession(container);
    }
}