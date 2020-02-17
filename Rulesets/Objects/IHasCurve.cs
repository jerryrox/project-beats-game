using UnityEngine;

namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Interface indicating that the implementing hit object has a curve.
	/// </summary>
	public interface IHasCurve : IHasDistance, IHasRepeats {

		/// <summary>
		/// Returns the path of the hit object.
		/// </summary>
		SliderPath Path { get; }
	}

	public static class HasCurveExtension {

		/// <summary>
		/// Returns the position on the path at specified time progress.
		/// </summary>
		public static Vector2 GetPosition(this IHasCurve context, float progress)
		{
			return context.Path.GetPosition(context.GetProgress(progress));
		}

		/// <summary>
		/// Returns the path progress from specified time progress which interpolates between start and end time.
		/// </summary>
		public static float GetProgress(this IHasCurve context, float progress)
		{
			float p = progress * context.SpanCount() % 1;
			// If is a repeat and is reversing back, invert progress towards end to start.
			if(context.GetSpan(progress) % 2 == 1)
				return 1 - p;
			return p;
		}

		/// <summary>
		/// Returns the path span index at specified path progress.
		/// </summary>
		public static int GetSpan(this IHasCurve context, float progress)
		{
			return (int)(progress * context.SpanCount());
		}
	}
}

