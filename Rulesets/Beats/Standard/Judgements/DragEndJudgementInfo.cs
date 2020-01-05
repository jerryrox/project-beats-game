using System;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Judgements
{
	/// <summary>
	/// Dragger end object's judgement is checked just for the dragger object to
	/// evaluate at the end, whether the user released the dragger too early or not.
	/// This judgement should not directly affect the performance in any way.
	/// </summary>
	public class DragEndJudgementInfo : Rulesets.Judgements.JudgementInfo {
		
		public override bool AffectsCombo { get { return false; } }

		public override int GetNumericResult (JudgementResult result) { return 0; }
	}
}

