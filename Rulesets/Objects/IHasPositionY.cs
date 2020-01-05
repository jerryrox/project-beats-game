namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Interface indicating that the implementing hit object has a Y position.
	/// </summary>
	public interface IHasPositionY {

		/// <summary>
		/// Returns the starting position Y of a hit object.
		/// </summary>
		float Y { get; }
	}
}

