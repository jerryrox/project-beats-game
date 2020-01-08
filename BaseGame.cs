using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBGame.Maps;
using PBGame.Audio;
using PBGame.Skins;
using PBGame.Stores;
using PBGame.Rulesets;
using PBGame.Assets.Caching;
using PBGame.Configurations;
using PBFramework.Audio;
using PBFramework.Dependencies;

namespace PBGame
{
    public abstract class BaseGame : MonoBehaviour, IGame {

        protected ModeManager modeManager;

        protected GameConfiguration gameConfiguration;
        protected MapConfiguration mapConfiguration;
        protected MapsetConfiguration mapsetConfiguration;

        protected MusicCacher musicCacher;
        protected BackgroundCacher backgroundCacher;

        protected SkinManager skinManager;

        protected MusicController musicController;
        protected SoundPooler soundPooler;

        protected MapsetStore mapsetStore;
        protected MapSelection mapSelection;
        protected MapManager mapManager;


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
            Dependencies.CacheAs<IModeManager>(modeManager = new ModeManager());

            Dependencies.CacheAs<IGameConfiguration>(gameConfiguration = new GameConfiguration());
            Dependencies.CacheAs<IMapConfiguration>(mapConfiguration = new MapConfiguration());
            Dependencies.CacheAs<IMapsetConfiguration>(mapsetConfiguration = new MapsetConfiguration());

            Dependencies.CacheAs<IMusicCacher>(musicCacher = new MusicCacher());
            Dependencies.CacheAs<IBackgroundCacher>(backgroundCacher = new BackgroundCacher());

            Dependencies.CacheAs<ISkinManager>(skinManager = new SkinManager());
            skinManager.DefaultSkin.AssetStore.Load();

            Dependencies.CacheAs<IMusicController>(musicController = MusicController.Create());
            Dependencies.CacheAs<ISoundPooler>(soundPooler = new SoundPooler(skinManager.DefaultSkin));

            Dependencies.CacheAs<IMapsetStore>(mapsetStore = new MapsetStore(modeManager));
            Dependencies.CacheAs<MapSelection>(mapSelection = new MapSelection(musicCacher, backgroundCacher));
            Dependencies.CacheAs<IMapManager>(mapManager = new MapManager(mapsetStore));
        }

        /// <summary>
        /// Handles final process after initialization.
        /// </summary>
        protected abstract void PostInitialize();
    }
}