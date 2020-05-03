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
		private static Dictionary<HitResultType, Tuple<float, float, float>> timingRanges = new Dictionary<HitResultType, Tuple<float, float, float>>() {
			{ HitResultType.Good, new Tuple<float, float, float>(160, 100, 40) },
			{ HitResultType.Ok, new Tuple<float, float, float>(280, 200, 120) },
			{ HitResultType.Bad, new Tuple<float, float, float>(400, 300, 200) },
			{ HitResultType.Miss, new Tuple<float, float, float>(400, 400, 400) }
		};


		public override void SetDifficulty (float difficulty)
		{
			Good = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Good]);
			Ok = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Ok]);
			Bad = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Bad]);
			Miss = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Miss]);
		}

		public override IEnumerable<HitResultType> SupportedHitResults ()
		{
			yield return HitResultType.Good;
			yield return HitResultType.Ok;
			yield return HitResultType.Bad;
			yield return HitResultType.Miss;
		}
	}
}

