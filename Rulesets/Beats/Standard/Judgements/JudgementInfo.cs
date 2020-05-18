using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Judgements
{
	/// <summary>
	/// Judgement information for Beats game mode.
	/// </summary>
	public class JudgementInfo : Rulesets.Judgements.JudgementInfo {

		public override HitResultType MaxResult { get { return HitResultType.Perfect; } }


		public override int GetNumericResult (HitResultType result)
		{
			switch(result)
			{
			case HitResultType.Perfect: return 150;
			case HitResultType.Great: return 50;
			case HitResultType.Good: return 25;
			}
			return 0;
		}

		public override float GetHealthBonus (HitResultType result)
		{
			switch(result)
            {
                case HitResultType.Perfect:
                    return 1f;
                case HitResultType.Great:
                    return 0.5f;
                case HitResultType.Good:
                    return 0.25f;
			}
			return 0f;
		}
	}
}