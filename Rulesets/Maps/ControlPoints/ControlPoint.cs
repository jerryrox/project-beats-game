using System;

namespace PBGame.Rulesets.Maps.ControlPoints
{
	/// <summary>
	/// Class which represents a timed point in a beatmap.
	/// </summary>
	public class ControlPoint : IComparable<ControlPoint>, IEquatable<ControlPoint> {

		/// <summary>
		/// Starting time of the control point.
		/// </summary>
		public double Time { get; set; }


        public int CompareTo (ControlPoint other) { return Time.CompareTo(other.Time); }

		public bool Equals (ControlPoint other) { return IsEquivalentTo(other) && Time.Equals(other.Time); }

		/// <summary>
		/// Returns whether specified point is treated equivalent to this point.
		/// </summary>
		public virtual bool IsEquivalentTo(ControlPoint other) { return true; }
	}
}