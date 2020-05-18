using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Judgements
{
	/// <summary>
	/// Judgement information for dragger tick in Beats game mode.
	/// </summary>
	public class TickJudgementInfo : JudgementInfo {
		
		public override int GetNumericResult (HitResultType result)
		{
			switch(result)
			{
			case HitResultType.Perfect:
			case HitResultType.Great:
			case HitResultType.Good:
				return 20;
			}
			return 0;
        }

        public override float GetHealthBonus(HitResultType result)
        {
            switch (result)
            {
                case HitResultType.Perfect:
                    return 0.25f;
            }
            return 0f;
        }
	}
}