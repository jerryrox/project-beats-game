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


        public ScoreProcessor() : base()
        {
        }

        public override void ApplyMap(IMap map)
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
            hpDrainRate = (float)map.Detail.Difficulty.HpDrainRate;
        }

		protected override float GetHealthChangeFactor (HitResults hitResult)
		{
            if(hitResult == HitResults.Miss) // Base: -0.02
				return (hpDrainRate * 0.7f) + 1f; // 1~8

            // Base: 0.01
            var factor = 10f - (Mathf.Clamp(hpDrainRate, 0f, 10f) * 0.9975f); // 10~40;
            switch(hitResult)
			{
                case HitResults.Good: return factor;
                case HitResults.Ok: return factor * 0.5f;
                case HitResults.Bad: return factor * 0.25f;
			}
			return 0;
		}
    }
}