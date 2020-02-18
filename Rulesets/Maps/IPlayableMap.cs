using PBGame.Rulesets.Difficulty;

namespace PBGame.Rulesets.Maps
{
    /// <summary>
    /// Indicates a converted version of original map instance ready for play in a specific game mode.
    /// </summary>
    public interface IPlayableMap : IMap, IComboColorable {
    
        /// <summary>
        /// Returns the original map this variant was derived from.
        /// </summary>
        IOriginalMap OriginalMap { get; }

        /// <summary>
        /// Returns the actual playable game mode of this map.
        /// </summary>
        GameModes PlayableMode { get; set; }

        /// <summary>
        /// Returns the difficulty information of this map.
        /// </summary>
        DifficultyInfo Difficulty { get; set; }
    }
}