using System;

namespace PBGame.Rulesets.Maps.ControlPoints
{
	/// <summary>
	/// Control point which manipulates gameplay effect.
	/// </summary>
	public class EffectControlPoint : ControlPoint, IComparable<EffectControlPoint> {

		/// <summary>
		/// Whether this control point is an highlight area.
		/// </summary>
		public bool IsHighlight { get; set; }


		public override bool IsEquivalentTo (ControlPoint other)
		{
			EffectControlPoint effectPoint = other as EffectControlPoint;
			if(effectPoint == null)
				return false;
			return base.IsEquivalentTo (other) &&
				IsHighlight.Equals(effectPoint.IsHighlight);
		}

		public int CompareTo(EffectControlPoint other)
		{
			return base.CompareTo(other);
		}
	}
}

