namespace PBGame.Rulesets.Maps.Timing
{
	public class BreakPoint {

		/// <summary>
		/// Minimum number of milliseconds which a break point can be valid from.
		/// </summary>
		public const double MinBreakDuration = 650;

		/// <summary>
		/// The starting time of this break point
		/// </summary>
		public double StartTime { get; set; }

		/// <summary>
		/// The ending time of this break point.
		/// </summary>
		public double EndTime { get; set; }


		/// <summary>
		/// Returns the duration of this break point.
		/// </summary>
		public double Duration => EndTime - StartTime;

        /// <summary>
        /// Returns whether this break point is valid and can provide breaks.
        /// </summary>
        public bool IsValid => (EndTime - StartTime) > MinBreakDuration;

        /// <summary>
        /// Returns whether a valid breakpoint can be created with specified time values.
        /// </summary>
        public static bool CanBeValid(double start, double end) => (end - start) > MinBreakDuration;
	}
}