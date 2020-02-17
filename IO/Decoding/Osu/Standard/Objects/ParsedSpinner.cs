using System;
using UnityEngine;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Judgements;

namespace PBGame.IO.Decoding.Osu.Standard.Objects
{
	/// <summary>
	/// Hit object variant used for parsing slider.
	/// </summary>
	public class ParsedSpinner : HitObject, IHasEndTime, IHasPosition, IHasCombo {
		
		public float EndTime { get; set; }

		public float Duration { get { return EndTime - StartTime; } }

		public Vector2 Position { get; set; }

		public float X { get { return Position.x; } }

		public float Y { get { return Position.y; } }

		public bool IsNewCombo { get; set; }

		public int ComboOffset { get; set; }

		protected override HitTiming CreateHitTiming () { return null; }
	}
}

