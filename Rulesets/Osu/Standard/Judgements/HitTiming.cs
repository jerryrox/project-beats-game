using System;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Osu.Standard.Judgements
{
	/// <summary>
	/// Provides timing values for Osu Standard hit objects.
	/// </summary>
	public class HitTiming : Rulesets.Judgements.HitTiming {

		/// <summary>
		/// The base hit timing values table.
		/// </summary>
		private static Dictionary<HitResultType, Tuple<float, float, float>> timingRanges = new Dictionary<HitResultType, Tuple<float, float, float>>() {
			{ HitResultType.Perfect, new Tuple<float, float, float>(160, 100, 40) },
			{ HitResultType.Great, new Tuple<float, float, float>(280, 200, 120) },
			{ HitResultType.Good, new Tuple<float, float, float>(400, 300, 200) },
			{ HitResultType.Miss, new Tuple<float, float, float>(400, 400, 400) }
		};


		public override void SetDifficulty (float difficulty)
		{
			Perfect = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Perfect]);
			Great = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Great]);
			Good = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Good]);
			Miss = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Miss]);
		}

		public override IEnumerable<HitResultType> SupportedHitResults ()
		{
			yield return HitResultType.Perfect;
			yield return HitResultType.Great;
			yield return HitResultType.Good;
			yield return HitResultType.Miss;
		}
	}
}

