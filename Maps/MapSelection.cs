using System;
using PBGame.Assets.Caching;
using PBGame.Rulesets.Maps;
using PBFramework.Audio;
using PBFramework.Allocation.Caching;

namespace PBGame.Maps
{
    public class MapSelection : IMapSelection {

        public event Action<IMapset> OnMapsetChange;

        public event Action<IMap> OnMapChange;

        public event Action<IMusicAudio> OnMusicLoaded;

        public event Action<IMapBackground> OnBackgroundLoaded;

        public event Action OnMusicUnloaded;

        public event Action OnBackgroundUnloaded;

        private IMusicCacher musicCacher;
        private IBackgroundCacher backgroundCacher;
        private ICacherAgent<IMap, IMusicAudio> musicAgent;
        private ICacherAgent<IMap, IMapBackground> backgroundAgent;

        private IMapBackground emptyBackground;


        public IMapset Mapset { get; private set; }

        public IMap Map { get; private set; }

        public IMusicAudio Music { get; private set; }

        public IMapBackground Background { get; private set; }

        public bool HasSelection => Mapset != null && Map != null;


        public MapSelection(IMusicCacher musicCacher, IBackgroundCacher backgroundCacher)
        {
            if(musicCacher == null) throw new ArgumentNullException(nameof(musicCacher));
            if(backgroundCacher == null) throw new ArgumentNullException(nameof(backgroundCacher));
            
            this.musicCacher = musicCacher;
            this.backgroundCacher = backgroundCacher;

            // Initial background.
            Background = emptyBackground = new MapBackground(null);

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
        }

        public void SelectMapset(IMapset mapset, IMap map = null)
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
            if (map == null) map = mapset.Maps[0];

            // Set mapset only if different.
            if (mapset != this.Mapset)
            {
                this.Mapset = mapset;
                OnMapsetChange?.Invoke(mapset);
            }

            // Select the map.
            SelectMap(map);
        }

        public void SelectMap(IMap map)
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
                this.Map = map;
                OnMapChange?.Invoke(map);

                // Switch or fresh-load the background and music.
                UnloadMusic();
                LoadMusic();
                UnloadBackground();
                LoadBackground();
            }
        }

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