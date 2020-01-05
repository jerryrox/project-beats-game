using System;
using System.Collections.Generic;
using PBFramework.Utils;

namespace PBGame.Rulesets.Maps
{
	/// <summary>
	/// Class which represents the difficulty section of a beatmap.
	/// </summary>
	public class MapDifficulty {

		public const double DefaultDifficulty = 5;
		public const double DefaultSliderDifficulty = 1;

        /// <summary>
        /// Seems like some osu maps do not contain a property for approach rate.
        /// </summary>
        private double? approachRate;


        public double HpDrainRate { get; set; } = DefaultDifficulty;

        public double CircleSize { get; set; } = DefaultDifficulty;

        public double OverallDifficulty { get; set; } = DefaultDifficulty;

        public double ApproachRate
        {
            get { return approachRate ?? OverallDifficulty; }
            set { approachRate = value; }
        }

        public double SliderMultiplier { get; set; } = DefaultSliderDifficulty;

        public double SliderTickRate { get; set; } = DefaultSliderDifficulty;

        /// <summary>
        /// Returns the actual difficulty value using given difficulty scale (0~10).
        /// values should contain values at difficulty 0, 5, and 10.
        /// </summary>
        public static double GetDifficultyValue(double difficulty, Tuple<double, double, double> values)
		{
			return GetDifficultyValue(difficulty, values.Item1, values.Item2, values.Item3);
		}

		/// <summary>
		/// Returns the actual difficulty value using given difficulty scale (0~10).
		/// values should contain values at difficulty 0, 5, and 10.
		/// </summary>
		public static double GetDifficultyValue(double difficulty, double d0, double d5, double d10)
		{
			if(difficulty > 5)
				return MathUtils.Lerp(d5, d10, (difficulty-5) / 5);
			else if(difficulty < 5)
				return MathUtils.Lerp(d0, d5, difficulty / 5);
			return d5;
		}
	}
}

