using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Judgements;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.Scoring
{
    public class ScoreProcessor : Rulesets.Scoring.ScoreProcessor {

        /// <summary>
        /// HP drain difficulty value.
        /// </summary>
        private float hpDrainRate;

        private float healthPerPerfect;


        protected override float HealthPerPerfect => healthPerPerfect;


        public ScoreProcessor() : base()
        {
        }

        public override void ApplyMap(IPlayableMap map)
        {
            base.ApplyMap(map);

            // Get total number of judgements
            maxJudgements = 0;
            foreach (var obj in map.HitObjects)
            {
                if (obj is HitCircle)
                {
                    maxJudgements++;
                    continue;
                }
                if (obj is Dragger dragger)
                {
                    maxJudgements += dragger.NestedObjects.Count + 1;
                    continue;
                }
            }

            // Get HP difficulty value.
            // TODO: Apply mod
            hpDrainRate = map.Detail.Difficulty.HpDrainRate;
            healthPerPerfect = 1f / Mathf.Min((int)(hpDrainRate * 15f + 25f), map.ObjectCount);
        }
    }
}