using System.Collections.Generic;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Osu.Catch.Judgements
{
	/// <summary>
	/// Provides timing values for Osu Catch hit objects.
	/// </summary>
	public class HitTiming : Rulesets.Judgements.HitTiming {

		public override IEnumerable<HitResultType> SupportedHitResults ()
		{
			yield return HitResultType.Perfect;
			yield return HitResultType.Miss;
		}
	}
}

