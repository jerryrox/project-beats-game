using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Judgements
{
	/// <summary>
	/// Judgement information for Beats game mode.
	/// </summary>
	public class JudgementInfo : Rulesets.Judgements.JudgementInfo {

		public override HitResults MaxResult { get { return HitResults.Good; } }


		protected override int GetNumericResult (HitResults result)
		{
			switch(result)
			{
			case HitResults.Good: return 150;
			case HitResults.Ok: return 50;
			case HitResults.Bad: return 25;
			}
			return 0;
		}

		protected override float GetHealthIncrease (HitResults result)
		{
			switch(result)
			{
			case HitResults.Miss:
				return -0.02f;
			case HitResults.Bad:
			case HitResults.Ok:
			case HitResults.Good:
				return 0.01f;
			}
			return 0f;
		}
	}
}