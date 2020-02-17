using System;
using UnityEngine;

namespace PBGame.Rulesets.Maps.ControlPoints
{
	/// <summary>
	/// Control point which manipulates difficulty.
	/// </summary>
	public class DifficultyControlPoint : ControlPoint, IComparable<DifficultyControlPoint> {

		private float speedMultiplier = 1;


		/// <summary>
		/// Speed multiplier of certain hit objects within this point.
		/// </summary>
		public float SpeedMultiplier
		{
			get { return speedMultiplier; }
			set { speedMultiplier = Mathf.Clamp(value, 0.1f, 10); }
		}


		public override bool IsEquivalentTo (ControlPoint other)
		{
			DifficultyControlPoint otherPoint = other as DifficultyControlPoint;
			if(otherPoint == null)
				return false;
			return base.IsEquivalentTo (other) &&
				speedMultiplier.Equals(otherPoint.speedMultiplier);
		}

		public int CompareTo(DifficultyControlPoint other)
		{
			return base.CompareTo(other);
		}
	}
}

