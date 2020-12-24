using System;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Osu.Taiko.Judgements
{
	/// <summary>
	/// Provides timing values for Osu Taiko hit objects.
	/// </summary>
	public class HitTiming : Rulesets.Judgements.HitTiming {

		/// <summary>
		/// The base hit timing values table.
		/// </summary>
		private static Dictionary<HitResultType, Tuple<float, float, float>> timingRanges = new Dictionary<HitResultType, Tuple<float, float, float>>() {
			{ HitResultType.Perfect, new Tuple<float, float, float>(100, 70, 40) },
			{ HitResultType.Good, new Tuple<float, float, float>(240, 80, 100) },
			{ HitResultType.Miss, new Tuple<float, float, float>(270, 190, 140) }
		};


		public override void SetDifficulty (float difficulty)
		{
			Perfect = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Perfect]);
			Good = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Good]);
			Miss = MapDifficulty.GetDifficultyValue(difficulty, timingRanges[HitResultType.Miss]);
		}

		public override IEnumerable<HitResultType> SupportedHitResults ()
		{
			yield return HitResultType.Perfect;
			yield return HitResultType.Good;
			yield return HitResultType.Miss;
		}
	}
}

