using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Osu.Standard
{
    public class ModeService : Rulesets.ModeService {

        public override string Name => "Osu Standard";

        public override string BaseIconName => "icon-mode-osu";

        public override GameModeType GameMode => GameModeType.OsuStandard;

        // TODO:
        public override bool IsPlayable => false;


        // TODO:
        public override Rulesets.Maps.IMapConverter CreateConverter(IOriginalMap map) => null;

        public override Rulesets.Maps.IMapProcessor CreateProcessor(IPlayableMap map) => new Standard.Maps.MapProcessor(map);

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