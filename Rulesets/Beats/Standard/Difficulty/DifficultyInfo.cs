using System;

namespace PBGame.Rulesets.Beats.Standard.Difficulty
{
    public class DifficultyInfo : Rulesets.Difficulty.DifficultyInfo {
		
        public override GameModes GameMode { get { return GameModes.BeatsStandard; } }

        /// <summary>
        /// The amount of strain value calculated in terms of speed.
        /// </summary>
        public double SpeedStrain { get; set; }
    }
}