namespace PBGame.Rulesets.Maps.Timing
{
	public class BreakPoint {

		/// <summary>
		/// Minimum number of milliseconds which a break point can be valid from.
		/// </summary>
		public const float MinBreakDuration = 650;

		/// <summary>
		/// The starting time of this break point
		/// </summary>
		public float StartTime { get; set; }

		/// <summary>
		/// The ending time of this break point.
		/// </summary>
		public float EndTime { get; set; }


		/// <summary>
		/// Returns the duration of this break point.
		/// </summary>
		public float Duration => EndTime - StartTime;

        /// <summary>
        /// Returns whether this break point is valid and can provide breaks.
        /// </summary>
        public bool IsValid => (EndTime - StartTime) > MinBreakDuration;

        /// <summary>
        /// Returns whether a valid breakpoint can be created with specified time values.
        /// </summary>
        public static bool CanBeValid(float start, float end) => (end - start) > MinBreakDuration;
	}
}