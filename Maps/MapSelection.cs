using System;
using PBGame.Assets.Caching;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBGame.Configurations.Maps;
using PBFramework.Data.Bindables;
using PBFramework.Audio;
using PBFramework.Allocation.Caching;

namespace PBGame.Maps
{
    public class MapSelection : IMapSelection {

        private Bindable<IMapset> bindableMapset = new Bindable<IMapset>(null);
        private Bindable<IPlayableMap> bindableMap = new Bindable<IPlayableMap>(null);
        private Bindable<IMusicAudio> bindableMusic = new Bindable<IMusicAudio>(null);
        private Bindable<IMapBackground> bindableBackground = new Bindable<IMapBackground>(null);

        private IMusicCacher musicCacher;
        private IBackgroundCacher backgroundCacher;
        private CacherAgent<IMap, IMusicAudio> musicAgent;
        private CacherAgent<IMap, IMapBackground> backgroundAgent;
        private IMapsetConfiguration mapsetConfiguration;
        private IMapConfiguration mapConfiguration;

        private IMapBackground emptyBackground;

        private GameModeType currentMode;


        public IReadOnlyBindable<IMapset> Mapset => bindableMapset;

        public IReadOnlyBindable<IPlayableMap> Map => bindableMap;

        public Bindable<MapsetConfig> MapsetConfig { get; private set; } = new Bindable<MapsetConfig>();

        public Bindable<MapConfig> MapConfig { get; private set; } = new Bindable<MapConfig>();

        public IReadOnlyBindable<IMusicAudio> Music => bindableMusic;

        public IReadOnlyBindable<IMapBackground> Background => bindableBackground;

        public bool HasSelection => bindableMapset.Value != null && bindableMap.Value != null;


        public MapSelection(IMusicCacher musicCacher,
            IBackgroundCacher backgroundCacher,
            IGameConfiguration gameConfiguration,
            IMapsetConfiguration mapsetConfiguration,
            IMapConfiguration mapConfiguration)
        {
            if(musicCacher == null) throw new ArgumentNullException(nameof(musicCacher));
            if(backgroundCacher == null) throw new ArgumentNullException(nameof(backgroundCacher));
            if(gameConfiguration == null) throw new ArgumentNullException(nameof(gameConfiguration));

            this.musicCacher = musicCacher;
            this.backgroundCacher = backgroundCacher;
            this.mapsetConfiguration = mapsetConfiguration;
            this.mapConfiguration = mapConfiguration;

            // Initial background.
            bindableBackground.Value = emptyBackground = new MapBackground(null);

            // Setup music loader
            musicAgent = new CacherAgent<IMap, IMusicAudio>(musicCacher)
            {
                UseDelayedRemove = true,
                RemoveDelay = 2f,
            };
            musicAgent.OnFinished += (music) =>
            {
                bindableMusic.Value = music;
            };

            // Setup background loader
            backgroundAgent = new CacherAgent<IMap, IMapBackground>(backgroundCacher)
            {
                UseDelayedRemove = true,
                RemoveDelay = 2f,
            };
            backgroundAgent.OnFinished += (background) =>
            {
                bindableBackground.Value = background;
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
                    if(Map.Value != null)
                        SelectMap(Map.Value.OriginalMap.GetPlayable(newMode));
                }
            };
        }

        public void SelectMapset(IMapset mapset, IPlayableMap map = null)
        {
            if (mapset == null)
            {
                UnloadMusic();
                UnloadBackground();
                return;
            }

            // Apply default map.
            if (map == null) {
                // Make sure the maps are sorted for the current game mode.
                mapset.SortMapsByMode(currentMode);
                map = mapset.Maps[0].GetPlayable(currentMode);
            }

            // Set mapset only if different.
            SetCurMapset(mapset, true);

            // Select the map.
            SelectMap(map);
        }

        public void SelectMap(IPlayableMap map)
        {
            if (map == null)
            {
                UnloadMusic();
                UnloadBackground();
                bindableMap.Value = null;
                return;
            }

            // Set map only if different.
            if (map != bindableMap.Value)
            {
                IPlayableMap prevMap = bindableMap.Value;
                SetCurMap(map, true);

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
        /// Sets current mapset state.
        /// </summary>
        private void SetCurMapset(IMapset mapset, bool onlyIfDifferent)
        {
            var previousMapset = bindableMapset.Value;
            if(onlyIfDifferent && previousMapset == mapset)
                return;

            bindableMapset.SetWithoutTrigger(mapset);
            this.MapsetConfig.Value = mapsetConfiguration?.GetConfig(mapset);
            bindableMapset.TriggerWithPrevious(previousMapset);
        }

        /// <summary>
        /// Sets current map state.
        /// </summary>
        private void SetCurMap(IPlayableMap map, bool onlyIfDifferent)
        {
            var previousMap = bindableMap.Value;
            if(onlyIfDifferent && previousMap == map)
                return;

            bindableMap.SetWithoutTrigger(map);
            this.MapConfig.Value = mapConfiguration?.GetConfig(map);
            bindableMap.TriggerWithPrevious(previousMap);
        }

        /// <summary>
        /// Loads the music asset for current map.
        /// </summary>
        private void LoadMusic()
        {
            if(bindableMap.Value != null)
                musicAgent.Request(bindableMap.Value);
        }

        /// <summary>
        /// Loads the background asset for current map.
        /// </summary>
        private void LoadBackground()
        {
            if(bindableMap.Value != null)
                backgroundAgent.Request(bindableMap.Value);
        }

        /// <summary>
        /// Unloads the loaded music asset.
        /// </summary>
        private void UnloadMusic()
        {
            musicAgent.Remove();
            bindableMusic.Value = null;
        }

        /// <summary>
        /// Unloads the loaded background asset.
        /// </summary>
        private void UnloadBackground()
        {
            backgroundAgent.Remove();
            bindableBackground.Value = emptyBackground;
        }
    }
}