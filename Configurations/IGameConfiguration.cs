using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.UI.Navigations.Screens.Songs;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations
{
    public interface IGameConfiguration : IConfiguration
    {
        /// <summary>
        /// The last selected ruleset mode.
        /// </summary>
        ProxyBindable<GameModes> RulesetMode { get; }
        /// <summary>
        /// The last selected mapset sorting field.
        /// </summary>
        ProxyBindable<MapsetSorts> MapsetSort { get; }
        /// <summary>
        /// The last selected tab for the map details section in SongsScreen.
        /// </summary>
        ProxyBindable<MapDetailTabs> MapDetailTab { get; }

        /// <summary>
        /// Login and displayed username of the player for online access.
        /// </summary>
        ProxyBindable<string> Username { get; }
        /// <summary>
        /// Login password of the player for online access.
        /// </summary>
		ProxyBindable<string> Password { get; }
        /// <summary>
        /// Whether the login username should be stored.
        /// </summary>
		ProxyBindable<bool> SaveUsername { get; }
        /// <summary>
        /// Whether the login password should be stored.
        /// </summary>
		ProxyBindable<bool> SavePassword { get; }

        /// <summary>
        /// Whether unicode texts should be preferred over english/romaji text.
        /// </summary>
		ProxyBindable<bool> PreferUnicode { get; }

        /// <summary>
        /// Overall volume of the game.
        /// </summary>
		ProxyBindable<float> MasterVolume { get; }
        /// <summary>
        /// Volume of the music.
        /// </summary>
		ProxyBindable<float> MusicVolume { get; }
        /// <summary>
        /// Volume of the hitsounds.
        /// </summary>
        ProxyBindable<float> HitsoundVolume { get; }
        /// <summary>
        /// Volume of the effect sounds.
        /// </summary>
		ProxyBindable<float> EffectVolume { get; }
        /// <summary>
        /// The global offset value in milliseconds.
        /// </summary>
		ProxyBindable<int> GlobalOffset { get; }
        /// <summary>
        /// Whether hitsounds from the beatmap should be used, if available.
        /// </summary>
		ProxyBindable<bool> UseBeatmapHitsounds { get; }

        /// <summary>
        /// Whether FPS counter should be displayed.
        /// </summary>
		ProxyBindable<bool> ShowFps { get; }
        /// <summary>
        /// Whether storyboard should be enabled during gameplay.
        /// </summary>
		ProxyBindable<bool> ShowStoryboard { get; }
        /// <summary>
        /// Whether video should be enabled during gameplay.
        /// </summary>
		ProxyBindable<bool> ShowVideo { get; }
        /// <summary>
        /// Whether to use the beatmap's skin elements if available.
        /// </summary>
		ProxyBindable<bool> UseBeatmapSkins { get; }
        /// <summary>
        /// Whether blur shader should be used.
        /// </summary>
		ProxyBindable<bool> UseBlurShader { get; }
        /// <summary>
        /// Percentage of the resolution quality.
        /// 1 = 100%, 0.5 = 50%.
        /// </summary>
		ProxyBindable<float> ResolutionQuality { get; }

        /// <summary>
        /// The global setting for background darkness, if not overridden by map configuration.
        /// </summary>
		ProxyBindable<float> BackgroundDim { get; }
    }
}