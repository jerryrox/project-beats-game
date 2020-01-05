using PBGame.Rulesets.Beats.Standard.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Objects
{
	/// <summary>
	/// Representation of a tick object on a dragger body.
	/// </summary>
	public class DraggerTick : HitObject {

		public override Rulesets.Judgements.JudgementInfo CreateJudgementInfo() { return new TickJudgementInfo(); }

		protected override Rulesets.Judgements.HitTiming CreateHitTiming() { return new HitTiming(); }
	}
}

