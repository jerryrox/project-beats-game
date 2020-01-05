using PBGame.Rulesets.Beats.Standard.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Objects
{
	/// <summary>
	/// Representation of a hit circle.
	/// </summary>
	public class HitCircle : HitObject {
		
		public override Rulesets.Judgements.JudgementInfo CreateJudgementInfo() { return new JudgementInfo(); }
	}
}

