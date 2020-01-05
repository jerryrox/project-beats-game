using PBGame.Rulesets.Beats.Standard.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Objects
{
	/// <summary>
	/// Representation of an ending hit object on a dragger.
	/// </summary>
	public class DraggerEndCircle : HitObject {

		public override Rulesets.Judgements.JudgementInfo CreateJudgementInfo() { return new DragEndJudgementInfo(); }

		protected override Rulesets.Judgements.HitTiming CreateHitTiming() { return new HitTiming(); }
	}
}

