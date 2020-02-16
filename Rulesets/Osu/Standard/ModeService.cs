using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Osu.Standard
{
    // TODO: Return correct instances for overridden methods.
    public class ModeService : Rulesets.ModeService {

        public override string Name => "Osu Standard";

        public override GameModes GameMode => GameModes.OsuStandard;


        public ModeService(IDependencyContainer dependency) : base(dependency)
        {
    
        }

        public override Rulesets.Maps.IMapConverter CreateConverter(IMap map) => null;

        public override Rulesets.Maps.IMapProcessor CreateProcessor(IMap map) => null;

        public override Rulesets.Difficulty.IDifficultyCalculator CreateDifficultyCalculator(IMap map) => null;

        public override Rulesets.Judgements.HitTiming CreateTiming() => null;

        protected override IGameSession CreateSession(IGraphicObject container) => null;
    }
}