using System;
using PBGame.Rulesets.Beats.Standard.Difficulty.Objects;
using PBGame.Rulesets.Difficulty.Skills;

namespace PBGame.Rulesets.Beats.Standard.Difficulty.Skills
{
    public class Speed : Skill
    {
        /// <summary>
        /// The amount of bonus times between two objects that provide extra bonus.
        /// </summary>
        private static readonly double[] BonusTimes = new double[] {
			100, // 150 BPM
            83.33333333, // 180 BPM
            62.5, // 240 BPM
            50 // 300 BPM
        };

		private const double DraggerMinBonus = 100; // 150 BPM
		private const double DraggerMaxBonus = 75; // 200 BPM


        protected override double StrainScore { get { return 10; } }

        protected override double StrainDecay { get { return 0.25; } }


		protected override double CalculateStrain(Rulesets.Difficulty.Objects.DifficultyHitObject obj)
        {
			var beatsObj = (DifficultyHitObject)obj;

			double deltaTime = Math.Max(BonusTimes[2], beatsObj.DeltaTime);

			double bonusTimeArea = BonusTimes[0];
			double speedBonus = 1;
			for(int i=BonusTimes.Length-2; i>=0; i--)
			{
				if(deltaTime <= BonusTimes[i])
				{
					bonusTimeArea = BonusTimes[i];
					speedBonus += (BonusTimes[0] - deltaTime) * (0.1 / bonusTimeArea);
					break;
				}
			}

			double draggerBonus = Math.Pow(1.02, beatsObj.DraggingCount) * (1.0 + (1.0 / bonusTimeArea));

			return speedBonus * draggerBonus;
        }
    }
}