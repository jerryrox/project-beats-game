using System;
using PBGame.Rulesets.Beats.Standard.Difficulty.Objects;
using PBGame.Rulesets.Difficulty.Skills;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.Difficulty.Skills
{
    public class Speed : Skill
    {
        /// <summary>
        /// The amount of bonus times between two objects that provide extra bonus.
        /// </summary>
        private static readonly float[] BonusTimes = new float[] {
			100, // 150 BPM
            83.33333333f, // 180 BPM
            62.5f, // 240 BPM
            50 // 300 BPM
        };

		private const float DraggerMinBonus = 100; // 150 BPM
		private const float DraggerMaxBonus = 75; // 200 BPM


        protected override float StrainScore { get { return 10; } }

        protected override float StrainDecay { get { return 0.25f; } }


		protected override float CalculateStrain(Rulesets.Difficulty.Objects.DifficultyHitObject obj)
        {
			var beatsObj = (DifficultyHitObject)obj;

			float deltaTime = Math.Max(BonusTimes[2], beatsObj.DeltaTime);

			float bonusTimeArea = BonusTimes[0];
			float speedBonus = 1;
			for(int i=BonusTimes.Length-2; i>=0; i--)
			{
				if(deltaTime <= BonusTimes[i])
				{
					bonusTimeArea = BonusTimes[i];
					speedBonus += (BonusTimes[0] - deltaTime) * (0.1f / bonusTimeArea);
					break;
				}
			}

			float draggerBonus = Mathf.Pow(1.02f, beatsObj.DraggingCount) * (1.0f + (1.0f / bonusTimeArea));

			return speedBonus * draggerBonus;
        }
    }
}