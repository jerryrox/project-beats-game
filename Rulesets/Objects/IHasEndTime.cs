namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Interface indicating that the implementing hit object has an end time.
	/// </summary>
	public interface IHasEndTime {

		/// <summary>
		/// Returns the ending time of the hit object.
		/// </summary>
		float EndTime { get; }

		/// <summary>
		/// Returns the duration of the hit object.
		/// </summary>
		float Duration { get; }
	}
}

