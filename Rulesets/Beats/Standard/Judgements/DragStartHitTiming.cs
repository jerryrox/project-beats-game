﻿using System;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Judgements
{
	/// <summary>
	/// Provides timing values for Beats dragger start hit object.
	/// </summary>
	public class DragStartHitTiming : HitTiming {

		/// <summary>
		/// The base hit timing values table.
		/// </summary>
		private static Dictionary<HitResults, Tuple<float, float, float>> timingRanges = new Dictionary<HitResults, Tuple<float, float, float>>() {
			{ HitResults.Good, new Tuple<float, float, float>(400, 300, 200) },
			{ HitResults.Miss, new Tuple<float, float, float>(400, 400, 400) }
		};


		public override void SetDifficulty (float difficulty)
		{
			Good = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResults.Good]);
			Miss = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResults.Miss]);
		}

		public override IEnumerable<HitResults> SupportedHitResults ()
		{
			yield return HitResults.Good;
			yield return HitResults.Miss;
		}
	}
}

