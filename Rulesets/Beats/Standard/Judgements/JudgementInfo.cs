using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Judgements
{
	/// <summary>
	/// Judgement information for Beats game mode.
	/// </summary>
	public class JudgementInfo : Rulesets.Judgements.JudgementInfo {

		public override HitResultType MaxResult { get { return HitResultType.Good; } }


		protected override int GetNumericResult (HitResultType result)
		{
			switch(result)
			{
			case HitResultType.Good: return 150;
			case HitResultType.Ok: return 50;
			case HitResultType.Bad: return 25;
			}
			return 0;
		}

		protected override float GetHealthIncrease (HitResultType result)
		{
			switch(result)
			{
			case HitResultType.Miss:
				return -0.02f;
			case HitResultType.Bad:
			case HitResultType.Ok:
			case HitResultType.Good:
				return 0.01f;
			}
			return 0f;
		}
	}
}