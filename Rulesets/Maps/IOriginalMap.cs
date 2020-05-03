namespace PBGame.Rulesets.Maps
{
    /// <summary>
    /// Indicates a map which can be converted to a playable variant.
    /// </summary>
    public interface IOriginalMap : IMap {

        /// <summary>
        /// Creates playable variants of this maps for modes included in specified manager.
        /// </summary>
        void CreatePlayable(IModeManager modeManager);

        /// <summary>
        /// Returns the playable map variant for specified game mode.
        /// If specified mode is not supported, it will return the variant of the game mode the map was created for.
        /// </summary>
        IPlayableMap GetPlayable(GameModeType gamemode);
    }
}