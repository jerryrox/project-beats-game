using PBGame.Rulesets.Beats.Standard.Judgements;

namespace PBGame.Rulesets.Beats.Standard.Objects
{
	public class DraggerStartCircle : HitCircle {

		protected override Rulesets.Judgements.HitTiming CreateHitTiming () { return new DragStartHitTiming(); }
	}
}

