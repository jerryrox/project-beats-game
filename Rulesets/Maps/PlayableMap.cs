using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Difficulty;
using UnityEngine;

namespace PBGame.Rulesets.Maps
{
    public class PlayableMap<T> : Map<T>, IPlayableMap
        where T : BaseHitObject
    {
        public IOriginalMap OriginalMap { get; private set; }

        public GameModeType PlayableMode { get; set; }

        public DifficultyInfo Difficulty { get; set; }

        public override MapDetail Detail => OriginalMap.Detail;

        public override ControlPointGroup ControlPoints => OriginalMap.ControlPoints;

        public override List<BreakPoint> BreakPoints => OriginalMap.BreakPoints;

        public override bool IsPlayable => true;

        public override List<Color> ComboColors => OriginalMap.ComboColors;


        public PlayableMap(IOriginalMap map)
        {
            OriginalMap = map;
        }
    }
}