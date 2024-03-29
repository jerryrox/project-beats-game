using System;
using PBGame.Maps;
using PBGame.Data.Rankings;
using PBGame.Rulesets;
using PBGame.Graphics;
using PBGame.Networking.API;
using PBGame.Notifications;
using PBGame.Configurations.Settings;
using PBFramework.Data.Bindables;
using PBFramework.Debugging;

namespace PBGame.Configurations
{
    public interface IGameConfiguration : IConfiguration
    {
        /// <summary>
        /// Event called on configuration load.
        /// </summary>
        event Action<IGameConfiguration> OnLoad;

        /// <summary>
        /// Event called on request for checking new mapsets in the download folder.
        /// </summary>
        event Action OnRequestMapsetCheck;

        /// <summary>
        /// Event called on request for opening game repository on browser.
        /// </summary>
        event Action OnRequestGameRepo;

        /// <summary>
        /// Event called on request for opening framework repository on browser.
        /// </summary>
        event Action OnRequestFrameworkRepo;

        /// <summary>
        /// Returns the settings data based on game configurations.
        /// </summary>
        ISettingsData Settings { get; }

        // ============================================================
        // Internal settings
        // ============================================================
        /// <summary>
        /// The last selected ruleset mode.
        /// </summary>
        ProxyBindable<GameModeType> RulesetMode { get; }

        /// <summary>
        /// The last selected mapset sorting field.
        /// </summary>
        ProxyBindable<MapsetSortType> MapsetSort { get; }

        /// <summary>
        /// The last selected type of rank retrieval source.
        /// </summary>
        ProxyBindable<RankDisplayType> RankDisplay { get; }

        /// <summary>
        /// Login and displayed username of the player for online access.
        /// </summary>
        ProxyBindable<string> Username { get; }

        /// <summary>
        /// Login password of the player for online access.
        /// </summary>
		ProxyBindable<string> Password { get; }

        /// <summary>
        /// Whether the login credentials should be stored.
        /// </summary>
		ProxyBindable<bool> SaveCredentials { get; }

        /// <summary>
        /// The API provider last used to log in with.
        /// </summary>
        ProxyBindable<ApiProviderType> LastLoginApi { get; }

        // ============================================================
        // General settings
        // ============================================================
        /// <summary>
        /// Whether unicode texts should be preferred over english/romaji text.
        /// </summary>
		ProxyBindable<bool> PreferUnicode { get; }
        /// <summary>
        /// Whether quick messages should be displayed.
        /// </summary>
        ProxyBindable<bool> DisplayMessages { get; }
        /// <summary>
        /// Whether quick message should be displayed in game.
        /// </summary>
        ProxyBindable<bool> DisplayMessagesInGame { get; }
        /// <summary>
        /// Whether parallax effect should be used.
        /// </summary>
        ProxyBindable<bool> UseParallax { get; }

        // ============================================================
        // Performance settings
        // ============================================================
        /// <summary>
        /// Whether FPS counter should be displayed.
        /// </summary>
		ProxyBindable<bool> ShowFps { get; }
        /// <summary>
        /// Whether blur shader should be used.
        /// </summary>
		ProxyBindable<bool> UseBlurShader { get; }
        /// <summary>
        /// Type of resolution quality.
        /// </summary>
		ProxyBindable<ResolutionType> ResolutionQuality { get; }
        /// <summary>
        /// Type of framerate setting.
        /// </summary>
        ProxyBindable<FramerateType> Framerate { get; }

        // ============================================================
        // Gameplay settings
        // ============================================================
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
        /// The global setting for background darkness, if not overridden by map configuration.
        /// </summary>
		ProxyBindableFloat BackgroundDim { get; }
        /// <summary>
        /// Whether to save replays along with records.
        /// </summary>
		ProxyBindable<bool> SaveReplays { get; }
        /// <summary>
        /// Whether failed records should be saved to the database.
        /// </summary>
		ProxyBindable<bool> SaveFailedRecords { get; }
        /// <summary>
        /// Whether replays should be saved for failed records.
        /// </summary>
		ProxyBindable<bool> SaveFailedReplays { get; }

        // ============================================================
        // Sound settings
        // ============================================================
        /// <summary>
        /// Overall volume of the game.
        /// </summary>
		ProxyBindableFloat MasterVolume { get; }
        /// <summary>
        /// Volume of the music.
        /// </summary>
		ProxyBindableFloat MusicVolume { get; }
        /// <summary>
        /// Volume of the hitsounds.
        /// </summary>
        ProxyBindableFloat HitsoundVolume { get; }
        /// <summary>
        /// Volume of the effect sounds.
        /// </summary>
		ProxyBindableFloat EffectVolume { get; }
        /// <summary>
        /// The global offset value in milliseconds.
        /// </summary>
		ProxyBindableInt GlobalOffset { get; }
        /// <summary>
        /// Whether hitsounds from the beatmap should be used, if available.
        /// </summary>
		ProxyBindable<bool> UseBeatmapHitsounds { get; }
        /// <summary>
        /// Whether button hover sounds should be played.
        /// </summary>
		ProxyBindable<bool> UseButtonHoverSound { get; }

        // ============================================================
        // Other settings
        // ============================================================
        /// <summary>
        /// The minimum level of notification type which makes the notification be sent to the stored scope.
        /// </summary>
        ProxyBindable<NotificationType> PersistNotificationLevel { get; }
        /// <summary>
        /// The minimum level of log message type which makes it displayed as a notification.
        /// </summary>
        ProxyBindable<LogType> LogToNotificationLevel { get; }
    }
}