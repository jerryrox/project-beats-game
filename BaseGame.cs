﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBGame.Maps;
using PBGame.Audio;
using PBGame.Skins;
using PBGame.Stores;
using PBGame.Assets.Fonts;
using PBGame.Assets.Caching;
using PBGame.Rulesets;
using PBGame.Graphics;
using PBGame.Animations;
using PBGame.Networking.API;
using PBGame.Configurations;
using PBFramework.UI.Navigations;
using PBFramework.Audio;
using PBFramework.Assets.Atlasing;
using PBFramework.Graphics;
using PBFramework.Services;
using PBFramework.Dependencies;

namespace PBGame
{
    public abstract class BaseGame : MonoBehaviour, IGame {

        protected ModeManager modeManager;

        protected GameConfiguration gameConfiguration;
        protected MapConfiguration mapConfiguration;
        protected MapsetConfiguration mapsetConfiguration;

        protected FontManager fontManager;
        protected IAtlas<Sprite> spriteAtlas;

        protected MusicCacher musicCacher;
        protected BackgroundCacher backgroundCacher;

        protected SkinManager skinManager;

        protected MusicController musicController;
        protected SoundPooler soundPooler;

        protected MapsetStore mapsetStore;
        protected MapSelection mapSelection;
        protected MapManager mapManager;
        protected Metronome metronome;

        protected DownloadStore downloadStore;
        protected ApiManager apiManager;

        protected IRootMain rootMain;
        protected IRoot3D root3D;
        protected IAnimePreset animePreset;
        protected IScreenNavigator screenNavigator;
        protected IOverlayNavigator overlayNavigator;


        public IDependencyContainer Dependencies { get; private set; } = new DependencyContainer(true);


        void Awake()
        {
            InitializeModules();

            PostInitialize();
        }

        public virtual void GracefulQuit()
        {

        }

        public virtual void ForceQuit()
        {
        }

        /// <summary>
        /// Initializes all required modules for the game.
        /// </summary>
        protected virtual void InitializeModules()
        {
            UnityThreadService.Initialize();

            Dependencies.CacheAs<IModeManager>(modeManager = new ModeManager());

            Dependencies.CacheAs<IGameConfiguration>(gameConfiguration = new GameConfiguration());
            Dependencies.CacheAs<IMapConfiguration>(mapConfiguration = new MapConfiguration());
            Dependencies.CacheAs<IMapsetConfiguration>(mapsetConfiguration = new MapsetConfiguration());

            Dependencies.CacheAs<IFontManager>(fontManager = new FontManager());
            Dependencies.CacheAs<IAtlas<Sprite>>(spriteAtlas = new ResourceSpriteAtlas());

            Dependencies.CacheAs<IMusicCacher>(musicCacher = new MusicCacher());
            Dependencies.CacheAs<IBackgroundCacher>(backgroundCacher = new BackgroundCacher());

            Dependencies.CacheAs<ISkinManager>(skinManager = new SkinManager());
            skinManager.DefaultSkin.AssetStore.Load();

            Dependencies.CacheAs<IMusicController>(musicController = MusicController.Create());
            Dependencies.CacheAs<ISoundPooler>(soundPooler = new SoundPooler(skinManager.DefaultSkin));

            Dependencies.CacheAs<IMapsetStore>(mapsetStore = new MapsetStore(modeManager));
            Dependencies.CacheAs<IMapSelection>(mapSelection = new MapSelection(musicCacher, backgroundCacher));
            Dependencies.CacheAs<IMapManager>(mapManager = new MapManager(mapsetStore));
            Dependencies.CacheAs<IMetronome>(metronome = new Metronome(mapSelection, musicController));

            Dependencies.CacheAs<IDownloadStore>(downloadStore = new DownloadStore());
            Dependencies.CacheAs<IApiManager>(apiManager = new ApiManager());

            Dependencies.CacheAs<IRootMain>(rootMain = RootMain.Create(Dependencies));
            Dependencies.CacheAs<IRoot3D>(root3D = Root3D.Create(Dependencies));
            Dependencies.CacheAs<IAnimePreset>(animePreset = new AnimePreset());
            Dependencies.CacheAs<IScreenNavigator>(screenNavigator = new ScreenNavigator(rootMain));
            Dependencies.CacheAs<IOverlayNavigator>(overlayNavigator = new OverlayNavigator(rootMain));
        }

        /// <summary>
        /// Handles final process after initialization.
        /// </summary>
        protected abstract void PostInitialize();

        protected virtual void Update()
        {
            metronome.Update();
        }
    }
}