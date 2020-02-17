using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PBFramework;
using PBFramework.Utils;

namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Slider path data of a slider type hit object.
	/// </summary>
	public class SliderPath : IEquatable<SliderPath> {

		/// <summary>
		/// Distance of the slider set in the osu file.
		/// </summary>
		public float? ExpectedDistance;

		/// <summary>
		/// Type of the path to be used by the slider.
		/// </summary>
		public PathTypes PathType;

		/// <summary>
		/// Finalized path of the slider.
		/// </summary>
		private List<Vector2> calculatedPath;

		/// <summary>
		/// Progressive lengths of the path at certain path points.
		/// </summary>
		private List<float> cumulativeLength;

		/// <summary>
		/// Whether the path was initialized.
		/// </summary>
		private bool isInitialized;

		private Vector2[] points;


		/// <summary>
		/// Returns the array of control points used to make up the path.
		/// </summary>
		public Vector2[] Points { get { return points; } }

		/// <summary>
		/// Returns the total distance of the path.
		/// </summary>
		public float Distance
		{
			get
			{
				return cumulativeLength.Count == 0 ? 0 : cumulativeLength[cumulativeLength.Count-1];
			}
		}


		public SliderPath(PathTypes type, Vector2[] points, float? expectedDistance = null)
		{
			this.points = points;
			PathType = type;
			this.ExpectedDistance = expectedDistance;

			Initialize();
		}

		/// <summary>
		/// Fills the specified list with the slider path values within the given progress.
		/// </summary>
		public void GetPath(List<Vector2> path, float startProgress, float endProgress)
		{
			float startDist = ProgressToDistance(startProgress);
			float endDist = ProgressToDistance(endProgress);

			path.Clear();

			int i = 0;
			while(i < calculatedPath.Count && cumulativeLength[i] < startDist)
				i ++;

			path.Add(InterpolateVertices(i, startDist));

			while(i < calculatedPath.Count && cumulativeLength[i] <= endDist)
				i ++;

			path.Add(InterpolateVertices(i, endDist));
		}

		/// <summary>
		/// Returns the position on the path at specified progress.
		/// </summary>
		public Vector2 GetPosition(float progress)
		{
			float dist = ProgressToDistance(progress);
			return InterpolateVertices(IndexOfDistance(dist), dist);
		}

		/// <summary>
		/// Initializes path if not already done.
		/// </summary>
		void Initialize()
		{
			if(isInitialized)
				return;

			isInitialized = true;

			points = points ?? new Vector2[0];
			calculatedPath = new List<Vector2>();
			cumulativeLength = new List<float>();

			CalculatePath();
			CalculateCumulativeLength();
		}

		/// <summary>
		/// Precalculates the entire path.
		/// </summary>
		void CalculatePath()
		{
			calculatedPath.Clear();

			int start = 0;
			int end = 0;
			for(int i=0; i<points.Length; i++)
			{
				end++;

				// In osu, if two consecutive points have the same position, it must start a new path.
				if(i == points.Length-1 || points[i] == points[i+1])
				{
					Vector2[] subPoints = points.Slice(start, end - start);

					foreach(var pos in CalculateSubpath(subPoints))
					{
						if(calculatedPath.Count == 0 || calculatedPath[calculatedPath.Count-1] != pos)
							calculatedPath.Add(pos);
					}

					start = end;
				}
			}
		}

		/// <summary>
		/// Precalculates the cumulative length of the entire path.
		/// </summary>
		void CalculateCumulativeLength()
		{
			float length = 0;

			cumulativeLength.Clear();
			cumulativeLength.Add(0);

			for(int i=0; i<calculatedPath.Count-1; i++)
			{
				Vector2 difference = calculatedPath[i+1] - calculatedPath[i];
				float d = difference.magnitude;

				// If the path is longer than expected, shorten the path.
				if(ExpectedDistance.HasValue && ExpectedDistance - length < d)
				{
					calculatedPath[i+1] = calculatedPath[i] + difference * (float)((ExpectedDistance - length) / d);
					calculatedPath.RemoveRange(i + 2, calculatedPath.Count - 2 - i);

					length = ExpectedDistance.Value;
					cumulativeLength.Add(length);
					break;
				}

				length += d;
				cumulativeLength.Add(length);
			}

			// If the path is shorter than expected, lengthen the path.
			if(ExpectedDistance.HasValue && length < ExpectedDistance && calculatedPath.Count > 1)
			{
				var difference = calculatedPath[calculatedPath.Count - 1] - calculatedPath[calculatedPath.Count - 2];
				float d = difference.magnitude;

				// If somehow equal position, just return.
				if(d <= 0)
					return;

				calculatedPath[calculatedPath.Count - 1] += difference * (float)((ExpectedDistance - length) / d);
				cumulativeLength[calculatedPath.Count-1] = ExpectedDistance.Value;
			}
		}

		/// <summary>
		/// Calculates the subpath inside a single path.
		/// Two consecutive control points at the same location indicates a subpath in osu.
		/// </summary>
		List<Vector2> CalculateSubpath(Vector2[] subPoints)
		{
			switch(PathType)
			{
			case PathTypes.Linear:
				return PathCalculator.ApproximateLinear(subPoints);
			case PathTypes.PerfectCurve:
				// Perfect curve types must only have 3 points.
				if(points.Length != 3 || subPoints.Length != 3)
					break;
				var subPath = PathCalculator.ApproximateCircularArc(subPoints);
				// If somehow not applicable, just break
				if(subPath.Count == 0)
					break;
				return subPath;
			case PathTypes.Catmull:
				return PathCalculator.CalculateCatmull(subPoints);
			}
			return PathCalculator.CalculateBezier(subPoints);
		}

		/// <summary>
		/// Returns the index relative to cumulative length list for specified distance.
		/// </summary>
		int IndexOfDistance(float distance)
		{
			int i = cumulativeLength.BinarySearch(distance);
			if(i < 0)
				i = ~i;
			return i;
		}

		/// <summary>
		/// Converts progress value from 0~1 to actual distance of the path.
		/// </summary>
		float ProgressToDistance(float progress)
		{
			return Mathf.Clamp01(progress) * Distance;
		}

		/// <summary>
		/// Returns the position on the slider path at specified index and distance.
		/// </summary>
		Vector2 InterpolateVertices(int i, float d)
		{
			if(calculatedPath.Count == 0)
				return Vector2.zero;
			if(i <= 0)
				return calculatedPath[0];
			if(i >= calculatedPath.Count)
				return calculatedPath[calculatedPath.Count - 1];

			Vector2 prevPoint = calculatedPath[i - 1];
			Vector2 curPoint = calculatedPath[i];
			float prevDistance = cumulativeLength[i - 1];
			float curDistance = cumulativeLength[i];

			// Prevent division by zero exception
			if(MathUtils.AlmostEquals(prevDistance, curDistance))
				return prevPoint;
			
			return prevPoint + (curPoint - prevPoint) * (float)((d - prevDistance) / (curDistance - prevDistance));
		}

		/// <summary>
		/// Returns whether this and other slider paths are identical.
		/// </summary>
		public bool Equals(SliderPath other)
		{
			if((points == null && other.points != null) || (other.points == null && points != null))
				return false;
			return points.SequenceEqual(other.points) && ExpectedDistance.Equals(other.ExpectedDistance) && PathType == other.PathType;
		}

		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj))
				return false;
			SliderPath other = obj as SliderPath;
			return other == null ? false : Equals(other);
		}
	}
}

