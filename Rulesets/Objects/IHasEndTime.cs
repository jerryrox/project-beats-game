namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Interface indicating that the implementing hit object has an end time.
	/// </summary>
	public interface IHasEndTime {

		/// <summary>
		/// Returns the ending time of the hit object.
		/// </summary>
		double EndTime { get; }

		/// <summary>
		/// Returns the duration of the hit object.
		/// </summary>
		double Duration { get; }
	}
}

