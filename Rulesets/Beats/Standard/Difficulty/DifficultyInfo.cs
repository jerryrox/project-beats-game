using System;

namespace PBGame.Rulesets.Beats.Standard.Difficulty
{
    public class DifficultyInfo : Rulesets.Difficulty.DifficultyInfo {
		
        public override GameModeType GameMode { get { return GameModeType.BeatsStandard; } }

        /// <summary>
        /// The amount of strain value calculated in terms of speed.
        /// </summary>
        public double SpeedStrain { get; set; }
    }
}