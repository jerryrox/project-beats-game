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
		private static Dictionary<HitResults, Tuple<double, double, double>> timingRanges = new Dictionary<HitResults, Tuple<double, double, double>>() {
			{ HitResults.Good, new Tuple<double, double, double>(160, 100, 40) },
			{ HitResults.Ok, new Tuple<double, double, double>(280, 200, 120) },
			{ HitResults.Bad, new Tuple<double, double, double>(400, 300, 200) },
			{ HitResults.Miss, new Tuple<double, double, double>(400, 400, 400) }
		};


		public override void SetDifficulty (double difficulty)
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

