using PBGame.Rulesets.Maps;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Osu.Taiko
{
    public class ModeService : Rulesets.ModeService {

        public override string Name => "Osu Taiko";

        public override string BaseIconName => "icon-mode-taiko";

        public override GameModeType GameMode => GameModeType.OsuTaiko;

        // TODO:
        public override bool IsPlayable => false;


        // TODO:
        public override Rulesets.Maps.IMapConverter CreateConverter(IOriginalMap map) => null;

        // TODO:
        public override Rulesets.Maps.IMapProcessor CreateProcessor(IPlayableMap map) => null;

        // TODO:
        public override Rulesets.Difficulty.IDifficultyCalculator CreateDifficultyCalculator(IPlayableMap map) => null;

        // TODO:
        public override Rulesets.Scoring.IScoreProcessor CreateScoreProcessor() => null;

        public override Rulesets.Judgements.HitTiming CreateTiming() => new Judgements.HitTiming();

        // TODO:
        protected override IGameSession CreateSession(IGraphicObject container, IDependencyContainer dependency)
        {
            return null;
        }
    }
}