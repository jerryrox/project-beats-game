using System;
using System.Linq;
using PBFramework.Data;

namespace PBGame.Rulesets.Maps.ControlPoints
{
    public class ControlPointGroup {

        public SortedList<TimingControlPoint> TimingPoints { get; private set; } = new SortedList<TimingControlPoint>();

        public SortedList<DifficultyControlPoint> DifficultyPoints { get; private set; } = new SortedList<DifficultyControlPoint>();

        public SortedList<SampleControlPoint> SamplePoints { get; private set; } = new SortedList<SampleControlPoint>();

        public SortedList<EffectControlPoint> EffectPoints { get; private set; } = new SortedList<EffectControlPoint>();

        /// <summary>
        /// Returns the highest bpm from all timing points.
        /// </summary>
        public double MaxBpm
		{
			get => 60000 / (TimingPoints.OrderBy((point) => point.BeatLength).FirstOrDefault() ?? new TimingControlPoint()).BeatLength;
		}

		/// <summary>
		/// Returns the lowest bpm from all timing points.
		/// </summary>
		public double MinBpm
		{
			get => 60000 / (TimingPoints.OrderByDescending((point) => point.BeatLength).FirstOrDefault() ?? new TimingControlPoint()).BeatLength;
		}

		/// <summary>
		/// Returns the most common bpm from all timing points.
		/// </summary>
		public double CommonBpm
		{
			get
			{
				var commonGroup = TimingPoints.GroupBy(point => point.BeatLength).OrderByDescending(g => g.Count()).FirstOrDefault();
				if(commonGroup != null)
					return (commonGroup.FirstOrDefault() ?? new TimingControlPoint()).BeatLength;
				return 60000 / new TimingControlPoint().BeatLength;
			}
		}


		/// <summary>
		/// Returns the timing control point which takes effect at specified time.
		/// </summary>
		public TimingControlPoint TimingPointAt(double time) { return SearchControlPoint(TimingPoints, time, TimingPoints.FirstOrDefault()); }

		/// <summary>
		/// Returns the difficulty control point which takes effect at specified time.
		/// </summary>
		public DifficultyControlPoint DifficultyPointAt(double time) { return SearchControlPoint(DifficultyPoints, time); }

		/// <summary>
		/// Returns the sample control point which takes effect at specified time.
		/// </summary>
		public SampleControlPoint SamplePointAt(double time) { return SearchControlPoint(SamplePoints, time, SamplePoints.Count > 0 ? SamplePoints[0] : null); }

		/// <summary>
		/// Returns the effect control point which takes effect at specified time.
		/// </summary>
		public EffectControlPoint EffectPointAt(double time) { return SearchControlPoint(EffectPoints, time, EffectPoints.Count > 0 ? EffectPoints[0] : null); }

		/// <summary>
		/// Returns the control point which takes effect at specified time.
		/// </summary>
		T SearchControlPoint<T>(SortedList<T> list, double time, T defaultFirstPoint = null) where T : ControlPoint, IComparable<T>, new()
		{
			// If nothing included in the list, return an empty point
			if(list.Count == 0)
				return new T();
			// If time before the first point, return an empty point
			if(time < list[0].Time)
				return defaultFirstPoint ?? new T();
			// If time after the last point, return the last point.
			if(time >= list[list.Count-1].Time)
				return list[list.Count-1];

			int startIndex = 1;
			int endIndex = list.Count-2;
			int midIndex = 0;
			while(startIndex <= endIndex)
			{
				midIndex = (endIndex + startIndex) >> 1;

				if(list[midIndex].Time < time)
					startIndex = midIndex + 1;
				else if(list[midIndex].Time > time)
					endIndex = midIndex - 1;
				else
					return list[midIndex];
			}
			return list[startIndex - 1];
		}
    }
}