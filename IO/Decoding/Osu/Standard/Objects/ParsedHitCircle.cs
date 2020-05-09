using UnityEngine;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Judgements;

namespace PBGame.IO.Decoding.Osu.Standard.Objects
{
	/// <summary>
	/// Hit object variant used for parsing hit circles
	/// </summary>
	public class ParsedHitCircle : BaseHitObject, IHasPosition, IHasCombo {

		public Vector2 Position { get; set; }

		public float X { get { return Position.x; } }

		public float Y { get { return Position.y; } }

		public bool IsNewCombo { get; set; }

		public int ComboOffset { get; set; }


		protected override HitTiming CreateHitTiming () { return null; }
	}
}

