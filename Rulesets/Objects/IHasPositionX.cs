namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Interface indicating that the implementing hit object has an X position.
	/// </summary>
	public interface IHasPositionX {

		/// <summary>
		/// Returns the starting position X of a hit object.
		/// </summary>
		float X { get; }
	}
}

