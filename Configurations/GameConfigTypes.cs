namespace PBGame.Configurations
{
    /// <summary>
    /// Types of fields contained in a game configuration.
    /// </summary>
    public enum GameConfigTypes {
    
        // =================== Convenience
        /// <summary>
        /// The last selected ruleset mode.
        /// </summary>
		RulesetMode,
        /// <summary>
        /// The last selected mapset sorting field.
        /// </summary>
		MapsetSort,
        /// <summary>
        /// The last selected tab for the map details section in SongsScreen.
        /// </summary>
		MapDetailTab,

        // =================== Online
        /// <summary>
        /// Login and displayed username of the player for online access.
        /// </summary>
		Username,
        /// <summary>
        /// Login password of the player for online access.
        /// </summary>
		Password,
        /// <summary>
        /// Whether the login username should be stored.
        /// </summary>
		SaveUsername,
        /// <summary>
        /// Whether the login password should be stored.
        /// </summary>
		SavePassword,

        // =================== General
        /// <summary>
        /// Whether unicode texts should be preferred over english/romaji text.
        /// </summary>
		PreferUnicode,

        // =================== Audio
        /// <summary>
        /// Overall volume of the game.
        /// </summary>
		MasterVolume,
        /// <summary>
        /// Volume of the music.
        /// </summary>
		MusicVolume,
        /// <summary>
        /// Volume of the hitsounds.
        /// </summary>
        HitsoundVolume,
        /// <summary>
        /// Volume of the effect sounds.
        /// </summary>
		EffectVolume,
        /// <summary>
        /// The global offset value in milliseconds.
        /// </summary>
		GlobalOffset,
        /// <summary>
        /// Whether hitsounds from the beatmap should be used, if available.
        /// </summary>
		UseBeatmapHitsounds,

        // =================== Graphics
        /// <summary>
        /// Whether FPS counter should be displayed.
        /// </summary>
		ShowFps,
        /// <summary>
        /// Whether storyboard should be enabled during gameplay.
        /// </summary>
		ShowStoryboard,
        /// <summary>
        /// Whether video should be enabled during gameplay.
        /// </summary>
		ShowVideo,
        /// <summary>
        /// Whether to use the beatmap's skin elements if available.
        /// </summary>
		UseBeatmapSkins,
        /// <summary>
        /// Whether blur shader should be used.
        /// </summary>
		UseBlurShader,
        /// <summary>
        /// Percentage of the resolution quality.
        /// 1 = 100%, 0.5 = 50%.
        /// </summary>
		ResolutionQuality,

        // =================== Gameplay
        /// <summary>
        /// The global setting for background darkness, if not overridden by map configuration.
        /// </summary>
		BackgroundDim,
    }
}