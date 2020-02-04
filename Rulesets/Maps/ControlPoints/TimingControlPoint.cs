using System;
using System.Collections.Generic;
using PBGame.Rulesets.Maps.Timing;
using PBFramework.Utils;

namespace PBGame.Rulesets.Maps.ControlPoints
{
	/// <summary>
	/// Control point which contains data related to beat timing.
	/// </summary>
	public class TimingControlPoint : ControlPoint, IComparable<TimingControlPoint> {

		/// <summary>
		/// The default length of a beat (1 second).
		/// </summary>
		public const double DefaultBeatLength = 1000;

		/// <summary>
		/// The time signature within this point.
		/// </summary>
		public TimeSignatures TimeSignature = TimeSignatures.Quadruple;

		private double beatLength = DefaultBeatLength;


		/// <summary>
		/// Length of a beat in milliseconds.
		/// </summary>
		public double BeatLength
		{
			get { return beatLength; }
			set { beatLength = MathUtils.Clamp(value, 6, 60000); }
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

