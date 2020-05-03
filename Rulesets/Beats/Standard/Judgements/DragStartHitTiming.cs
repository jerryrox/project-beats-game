using System;
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
		private static Dictionary<HitResultType, Tuple<float, float, float>> timingRanges = new Dictionary<HitResultType, Tuple<float, float, float>>() {
			{ HitResultType.Good, new Tuple<float, float, float>(400, 300, 200) },
			{ HitResultType.Miss, new Tuple<float, float, float>(400, 400, 400) }
		};


		public override void SetDifficulty (float difficulty)
		{
			Good = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Good]);
			Miss = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Miss]);
		}

		public override IEnumerable<HitResultType> SupportedHitResults ()
		{
			yield return HitResultType.Good;
			yield return HitResultType.Miss;
		}
	}
}

