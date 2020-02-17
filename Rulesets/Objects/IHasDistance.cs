namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Interface indicating that the implementing hit object has a distance.
	/// </summary>
	public interface IHasDistance : IHasEndTime {

		/// <summary>
		/// Returns the positional length of the hit object.
		/// </summary>
		float Distance { get; }
	}
}

