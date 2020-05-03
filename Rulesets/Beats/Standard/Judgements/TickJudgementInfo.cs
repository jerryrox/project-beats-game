using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Judgements
{
	/// <summary>
	/// Judgement information for dragger tick in Beats game mode.
	/// </summary>
	public class TickJudgementInfo : JudgementInfo {
		
		protected override int GetNumericResult (HitResultType result)
		{
			switch(result)
			{
			case HitResultType.Good:
			case HitResultType.Ok:
			case HitResultType.Bad:
				return 20;
			}
			return 0;
		}
	}
}