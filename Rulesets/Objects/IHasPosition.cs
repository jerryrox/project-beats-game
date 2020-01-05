using UnityEngine;

namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Interface indicating that the implementing hit object has a position.
	/// </summary>
	public interface IHasPosition : IHasPositionX, IHasPositionY {

		/// <summary>
		/// Returns the starting position of a hit object.
		/// </summary>
		Vector2 Position { get; }
	}
}

