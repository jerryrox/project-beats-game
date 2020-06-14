using System;
using PBGame.Rulesets.Maps.Timing;
using UnityEngine;

namespace PBGame.Rulesets.Maps.ControlPoints
{
	/// <summary>
	/// Control point which contains data related to beat timing.
	/// </summary>
	public class TimingControlPoint : ControlPoint, IComparable<TimingControlPoint> {

		/// <summary>
		/// The default length of a beat (1 second).
		/// </summary>
		public const float DefaultBeatLength = 1000;

		/// <summary>
		/// The time signature within this point.
		/// </summary>
		public TimeSignatureType TimeSignature = TimeSignatureType.Quadruple;

		private float beatLength = DefaultBeatLength;


		/// <summary>
		/// Length of a beat in milliseconds.
		/// </summary>
		public float BeatLength
		{
			get { return beatLength; }
			set { beatLength = Mathf.Clamp(value, 6, 60000); }
		}


		public override bool IsEquivalentTo (ControlPoint other)
		{
			TimingControlPoint timingPoint = other as TimingControlPoint;
			if(timingPoint == null)
				return false;
			return base.IsEquivalentTo (other) &&
				TimeSignature.Equals(timingPoint.TimeSignature) &&
				beatLength.Equals(timingPoint.beatLength);
		}

		public int CompareTo(TimingControlPoint other)
		{
			return base.CompareTo(other);
		}

        public override string ToString()
        {
			return $"TimingPoint(Time={Time}, TimeSignature={TimeSignature}, BeatLength={BeatLength})";
        }
    }
}

