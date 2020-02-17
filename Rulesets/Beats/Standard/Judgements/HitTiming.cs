using System;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Judgements
{
	/// <summary>
	/// Provides timing values for Beats hit objects.
	/// </summary>
	public class HitTiming : Rulesets.Judgements.HitTiming {

		/// <summary>
		/// The base hit timing values table.
		/// </summary>
		private static Dictionary<HitResults, Tuple<float, float, float>> timingRanges = new Dictionary<HitResults, Tuple<float, float, float>>() {
			{ HitResults.Good, new Tuple<float, float, float>(160, 100, 40) },
			{ HitResults.Ok, new Tuple<float, float, float>(280, 200, 120) },
			{ HitResults.Bad, new Tuple<float, float, float>(400, 300, 200) },
			{ HitResults.Miss, new Tuple<float, float, float>(400, 400, 400) }
		};


		public override void SetDifficulty (float difficulty)
		{
			Good = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResults.Good]);
			Ok = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResults.Ok]);
			Bad = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResults.Bad]);
			Miss = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResults.Miss]);
		}

		public override IEnumerable<HitResults> SupportedHitResults ()
		{
			yield return HitResults.Good;
			yield return HitResults.Ok;
			yield return HitResults.Bad;
			yield return HitResults.Miss;
		}
	}
}

