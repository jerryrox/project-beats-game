using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Beats.Standard.Maps;
using PBGame.Rulesets.Beats.Standard.Scoring;
using PBGame.Rulesets.Beats.Standard.Difficulty;
using PBGame.Rulesets.Beats.Standard.Judgements;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public class ModeService : Rulesets.ModeService {

        public override string Name => "Beats Standard";

        public override string BaseIconName => "icon-mode-beats";

        public override GameModeType GameMode => GameModeType.BeatsStandard;

        public override int LatestReplayVersion => 2;


        public override Rulesets.Maps.IMapConverter CreateConverter(IOriginalMap map) => new MapConverter(map);

        public override Rulesets.Maps.IMapProcessor CreateProcessor(IPlayableMap map) => new Standard.Maps.MapProcessor(map);

        public override Rulesets.Difficulty.IDifficultyCalculator CreateDifficultyCalculator(IPlayableMap map) => new DifficultyCalculator(map);

        public override Rulesets.Scoring.IScoreProcessor CreateScoreProcessor() => new ScoreProcessor();

        public override Rulesets.Judgements.HitTiming CreateTiming() => new HitTiming();

        protected override IGameSession CreateSession(IGraphicObject container, IDependencyContainer dependency)
        {
            var session = new BeatsStandardSession(container);
            dependency.Inject(session);
            return session;
        }
    }
}