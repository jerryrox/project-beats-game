using System;
using PBGame.Assets.Caching;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.Audio;
using PBFramework.Allocation.Caching;

namespace PBGame.Maps
{
    public class MapSelection : IMapSelection {

        public event Action<IMapset> OnMapsetChange;

        public event Action<IPlayableMap> OnMapChange;

        public event Action<IMusicAudio> OnMusicLoaded;

        public event Action<IMapBackground> OnBackgroundLoaded;

        public event Action OnMusicUnloaded;

        public event Action OnBackgroundUnloaded;

        private IMusicCacher musicCacher;
        private IBackgroundCacher backgroundCacher;
        private ICacherAgent<IMap, IMusicAudio> musicAgent;
        private ICacherAgent<IMap, IMapBackground> backgroundAgent;

        private IMapBackground emptyBackground;

        private GameModeType currentMode;


        public IMapset Mapset { get; private set; }

        public IPlayableMap Map { get; private set; }

        public IMusicAudio Music { get; private set; }

        public IMapBackground Background { get; private set; }

        public bool HasSelection => Mapset != null && Map != null;


        public MapSelection(IMusicCacher musicCacher,
            IBackgroundCacher backgroundCacher,
            IGameConfiguration gameConfiguration)
        {
            if(musicCacher == null) throw new ArgumentNullException(nameof(musicCacher));
            if(backgroundCacher == null) throw new ArgumentNullException(nameof(backgroundCacher));
            if(gameConfiguration == null) throw new ArgumentNullException(nameof(gameConfiguration));

            this.musicCacher = musicCacher;
            this.backgroundCacher = backgroundCacher;

            // Initial background.
            Background = emptyBackground = new MapBackground(null);

            // Setup music loader
            musicAgent = new CacherAgent<IMap, IMusicAudio>(musicCacher)
            {
                UseDelayedRemove = true,
                RemoveDelay = 2f,
            };
            musicAgent.OnFinished += (music) =>
            {
                this.Music = music;
                OnMusicLoaded?.Invoke(music);
            };

            // Setup background loader
            backgroundAgent = new CacherAgent<IMap, IMapBackground>(backgroundCacher)
            {
                UseDelayedRemove = true,
                RemoveDelay = 2f,
            };
            backgroundAgent.OnFinished += (background) =>
            {
                this.Background = background;
                OnBackgroundLoaded?.Invoke(background);
            };

            // Listen to game mode change event from config.
            gameConfiguration.OnLoad += delegate
            {
                // Retrieve initial game mode saved in config.
                currentMode = gameConfiguration.RulesetMode.Value;
            };
            gameConfiguration.RulesetMode.OnValueChanged += (newMode, oldMode) =>
            {
                // Only if change of mode
                if (oldMode != newMode)
                {
                    currentMode = newMode;
                    // Automatically change to variant playable for this new mode.
                    if(Map != null)
                        SelectMap(Map.OriginalMap.GetPlayable(newMode));
                }
            };
        }

        public void SelectMapset(IMapset mapset, IPlayableMap map = null)
        {
            if (mapset == null)
            {
                UnloadMusic();
                UnloadBackground();
                OnMapsetChange?.Invoke(null);
                OnMapChange?.Invoke(null);
                return;
            }

            // Apply default map.
            if (map == null) map = mapset.Maps[0].GetPlayable(currentMode);

            // Set mapset only if different.
            if (mapset != this.Mapset)
            {
                this.Mapset = mapset;
                OnMapsetChange?.Invoke(mapset);
            }

            // Select the map.
            SelectMap(map);
        }

        public void SelectMap(IPlayableMap map)
        {
            if (map == null)
            {
                UnloadMusic();
                UnloadBackground();
                OnMapChange?.Invoke(null);
                return;
            }

            // Set map only if different.
            if (map != this.Map)
            {
                IPlayableMap prevMap = this.Map;
                this.Map = map;
                OnMapChange?.Invoke(map);

                // Change background / audio assets when necessary.
                if (prevMap == null || !prevMap.Detail.IsSameBackground(map.Detail))
                {
                    UnloadBackground();
                    LoadBackground();
                }
                if (prevMap == null || !prevMap.Detail.IsSameAudio(map.Detail))
                {
                    UnloadMusic();
                    LoadMusic();
                }
            }
        }

        public void SelectMap(IOriginalMap map) => SelectMap(map.GetPlayable(currentMode));

        /// <summary>
        /// Loads the music asset for current map.
        /// </summary>
        private void LoadMusic()
        {
            if(Map != null)
                musicAgent.Request(Map);
        }

        /// <summary>
        /// Loads the background asset for current map.
        /// </summary>
        private void LoadBackground()
        {
            if(Map != null)
                backgroundAgent.Request(Map);
        }

        /// <summary>
        /// Unloads the loaded music asset.
        /// </summary>
        private void UnloadMusic()
        {
            musicAgent.Remove();
            Music = null;
            OnMusicUnloaded?.Invoke();
        }

        /// <summary>
        /// Unloads the loaded background asset.
        /// </summary>
        private void UnloadBackground()
        {
            backgroundAgent.Remove();
            Background = null;
            OnBackgroundUnloaded?.Invoke();

            Background = emptyBackground;
            OnBackgroundLoaded?.Invoke(Background);
        }
    }
}