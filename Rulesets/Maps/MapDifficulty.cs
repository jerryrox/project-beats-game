using System;
using System.Collections.Generic;
using PBFramework.Utils;
using UnityEngine;

namespace PBGame.Rulesets.Maps
{
	/// <summary>
	/// Class which represents the difficulty section of a beatmap.
	/// </summary>
	public class MapDifficulty {

		public const float DefaultDifficulty = 5;
		public const float DefaultSliderDifficulty = 1;

        /// <summary>
        /// Seems like some osu maps do not contain a property for approach rate.
        /// </summary>
        private float? approachRate;


        public float HpDrainRate { get; set; } = DefaultDifficulty;

        public float CircleSize { get; set; } = DefaultDifficulty;

        public float OverallDifficulty { get; set; } = DefaultDifficulty;

        public float ApproachRate
        {
            get { return approachRate ?? OverallDifficulty; }
            set { approachRate = value; }
        }

        public float SliderMultiplier { get; set; } = DefaultSliderDifficulty;

        public float SliderTickRate { get; set; } = DefaultSliderDifficulty;

        /// <summary>
        /// Returns the actual difficulty value using given difficulty scale (0~10).
        /// values should contain values at difficulty 0, 5, and 10.
        /// </summary>
        public static float GetDifficultyValue(float difficulty, Tuple<float, float, float> values)
		{
			return GetDifficultyValue(difficulty, values.Item1, values.Item2, values.Item3);
		}

		/// <summary>
		/// Returns the actual difficulty value using given difficulty scale (0~10).
		/// values should contain values at difficulty 0, 5, and 10.
		/// </summary>
		public static float GetDifficultyValue(float difficulty, float d0, float d5, float d10)
		{
			if(difficulty > 5)
				return Mathf.Lerp(d5, d10, (difficulty-5) / 5);
			else if(difficulty < 5)
				return Mathf.Lerp(d0, d5, difficulty / 5);
			return d5;
		}
	}
}

