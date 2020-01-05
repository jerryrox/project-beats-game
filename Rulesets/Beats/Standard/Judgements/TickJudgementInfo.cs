using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Judgements
{
	/// <summary>
	/// Judgement information for dragger tick in Beats game mode.
	/// </summary>
	public class TickJudgementInfo : JudgementInfo {
		
		protected override int GetNumericResult (HitResults result)
		{
			switch(result)
			{
			case HitResults.Good:
			case HitResults.Ok:
			case HitResults.Bad:
				return 20;
			}
			return 0;
		}
	}
}