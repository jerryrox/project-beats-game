using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBGame.IO.Decoding.Osu;
using PBGame.UI;
using PBGame.Maps;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Audio;
using PBGame.Stores;
using PBGame.Assets.Fonts;
using PBGame.Assets.Caching;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Graphics;
using PBGame.Animations;
using PBGame.Networking.API;
using PBGame.Notifications;
using PBGame.Configurations;
using PBFramework.IO.Decoding;
using PBFramework.UI.Navigations;
using PBFramework.Utils;
using PBFramework.Audio;
using PBFramework.Assets.Atlasing;
using PBFramework.Inputs;
using PBFramework.Services;
using PBFramework.Dependencies;

namespace PBGame
{
    public abstract class BaseGame : MonoBehaviour, IGame {

        public event Action<bool> OnAppFocus;
        public event Action<bool> OnAppPause;

        protected ModeManager modeManager;

        protected NotificationBox notificationBox;

        protected GameConfiguration gameConfiguration;
        protected MapConfiguration mapConfiguration;
        protected MapsetConfiguration mapsetConfiguration;

        protected FontManager fontManager;
        protected ResourceSpriteAtlas spriteAtlas;
        protected ResourceAudioAtlas audioAtlas;

        protected MusicCacher musicCacher;
        protected BackgroundCacher backgroundCacher;
        protected WebImageCacher webImageCacher;
        protected WebMusicCacher webMusicCacher;

        protected MusicController musicController;

        protected DefaultSoundTable soundTable;
        protected SoundPool soundPool;

        protected MapsetStore mapsetStore;
        protected MapSelection mapSelection;
        protected MapManager mapManager;
        protected Metronome metronome;

        protected DownloadStore downloadStore;
        protected ApiManager apiManager;

        protected IUserManager userManager;
        protected IRecordManager recordManager;

        protected IRootMain rootMain;
        protected IRoot3D root3D;
        protected IColorPreset colorPreset;
        protected IAnimePreset animePreset;
        protected IScreenNavigator screenNavigator;
        protected IOverlayNavigator overlayNavigator;
        protected IDropdownProvider dropdownProvider;

        protected InputManager inputManager;


        public IDependencyContainer Dependencies { get; private set; } = new DependencyContainer(true);

        public string Version => Application.version;


        void Awake()
        {
            InitializeModules();

            PostInitialize();
        }

        public virtual void GracefulQuit()
        {
            ForceQuit();
        }

        public virtual void ForceQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Initializes all required modules for the game.
        /// </summary>
        protected virtual void InitializeModules()
        {
            UnityThreadService.Initialize();

            Dependencies.CacheAs<IGame>(this);

            Dependencies.CacheAs<IModeManager>(modeManager = new ModeManager());

            Dependencies.CacheAs<INotificationBox>(notificationBox = new NotificationBox());

            Dependencies.CacheAs<IGameConfiguration>(gameConfiguration = new GameConfiguration());
            Dependencies.CacheAs<IMapConfiguration>(mapConfiguration = new MapConfiguration());
            Dependencies.CacheAs<IMapsetConfiguration>(mapsetConfiguration = new MapsetConfiguration());

            Dependencies.CacheAs<IFontManager>(fontManager = new FontManager());
            Dependencies.CacheAs<IAtlas<Sprite>>(spriteAtlas = new ResourceSpriteAtlas());
            Dependencies.CacheAs<IAtlas<AudioClip>>(audioAtlas = new ResourceAudioAtlas());

            Dependencies.CacheAs<IMusicCacher>(musicCacher = new MusicCacher());
            Dependencies.CacheAs<IBackgroundCacher>(backgroundCacher = new BackgroundCacher());
            Dependencies.CacheAs<IWebImageCacher>(webImageCacher = new WebImageCacher());
            Dependencies.CacheAs<IWebMusicCacher>(webMusicCacher = new WebMusicCacher());

            Dependencies.CacheAs<IMusicController>(musicController = MusicController.Create());

            Dependencies.CacheAs<ISoundTable>(soundTable = new DefaultSoundTable(audioAtlas));
            Dependencies.CacheAs<ISoundPool>(soundPool = new SoundPool(soundTable));

            Dependencies.CacheAs<IMapsetStore>(mapsetStore = new MapsetStore(modeManager));
            Dependencies.CacheAs<IMapSelection>(mapSelection = new MapSelection(musicCacher, backgroundCacher, gameConfiguration));
            Dependencies.CacheAs<IMapManager>(mapManager = new MapManager(mapsetStore, notificationBox));
            Dependencies.CacheAs<IMetronome>(metronome = new Metronome(mapSelection, musicController));

            Dependencies.CacheAs<IDownloadStore>(downloadStore = new DownloadStore());
            Dependencies.CacheAs<IApiManager>(apiManager = new ApiManager());

            Dependencies.CacheAs<IUserManager>(userManager = new UserManager(Dependencies));
            Dependencies.CacheAs<IRecordManager>(recordManager = new RecordManager(Dependencies));

            Dependencies.CacheAs<IRootMain>(rootMain = RootMain.Create(Dependencies));
            Dependencies.CacheAs<IRoot3D>(root3D = Root3D.Create(Dependencies));
            Dependencies.CacheAs<IColorPreset>(colorPreset = new ColorPreset());
            Dependencies.CacheAs<IAnimePreset>(animePreset = new AnimePreset());
            Dependencies.CacheAs<IScreenNavigator>(screenNavigator = new ScreenNavigator(rootMain));
            Dependencies.CacheAs<IOverlayNavigator>(overlayNavigator = new OverlayNavigator(rootMain));
            Dependencies.CacheAs<IDropdownProvider>(dropdownProvider = new DropdownProvider(rootMain));

            Dependencies.CacheAs<IInputManager>(inputManager = InputManager.Create(rootMain.Resolution, Application.isMobilePlatform ? 0 : 2));
        }

        protected virtual void OnApplicationPause(bool paused) => OnAppPause?.Invoke(paused);

        protected virtual void OnApplicationFocus(bool focused) => OnAppFocus?.Invoke(focused);

        /// <summary>
        /// Handles final process after initialization.
        /// </summary>
        protected virtual void PostInitialize()
        {
            // Some default system settings.
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;

            // Inject notification box into api manager
            foreach(var api in apiManager.GetAllApi())
                api.NotificationBox = notificationBox;

            // Apply accelerator to input manager
            inputManager.Accelerator = (Application.isMobilePlatform ? (IAccelerator)new DeviceAccelerator() : (IAccelerator)new CursorAccelerator());

            // Register decoders.
            Decoders.AddDecoder<OriginalMap>(
                "osu file format v",
                (header) => new OsuBeatmapDecoder(ParseUtils.ParseInt(header.Split('v').Last(), OsuBeatmapDecoder.LatestVersion))
            );
        }

        protected virtual void Update()
        {
            metronome.Update();
        }


    }
}