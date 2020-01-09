namespace PBGame.Rulesets.Maps
{
	/// <summary>
	/// Interface of a converter which converts a map into a map of a different game mode than intended.
	/// </summary>
	public interface IMapConverter {

		/// <summary>
		/// Returns the map in conversion.
		/// </summary>
		IMap Map { get; }

		/// <summary>
		/// Returns whether current map can be converted.
		/// </summary>
		bool IsConvertible { get; }

        /// <summary>
        /// Returns the game mode which the map is being converted for.
        /// </summary>
        GameModes TargetMode { get; }


        /// <summary>
        /// Converts current map and returns a new map representing it.
        /// </summary>
        IMap Convert();
	}
}

