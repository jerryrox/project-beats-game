using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBFramework;
using PBFramework.Utils;
using PBFramework.Audio;
using PBFramework.Networking;

namespace PBGame.Stores
{
    public class MapAssetStore : AssetStore, IMapAssetStore, IDisposable
    {
        private Dictionary<string, HitsoundLoadInfo> hitsoundInfos = new Dictionary<string, HitsoundLoadInfo>();
        private MultiPromise hitsoundLoader;


        public bool IsHitsoundLoaded { get; private set; }

        public IPlayableMap Map { get; private set; }

        public MapSoundTable SoundTable { get; private set; }


        public MapAssetStore(IPlayableMap map, ISoundTable fallbackSoundTable) : base(map.Detail.Mapset.Directory)
        {
            Map = map;
            SoundTable = new MapSoundTable(fallbackSoundTable);
        }

        public IExplicitPromise LoadHitsounds()
        {
            DisposeHitsoundLoader();

            if(IsHitsoundLoaded)
                return new ProxyPromise((resolve) => resolve(null));

            FindHitsounds();

            // Load all audio clips from storage.
            hitsoundLoader = new MultiPromise(CreateHitsoundLoaders());
            hitsoundLoader.OnFinished += () =>
            {
                IsHitsoundLoaded = true;
                hitsoundLoader = null;
            };
            // Return the loader promise.
            return hitsoundLoader;
        }

        public void Dispose()
        {
            DisposeHitsoundLoader();
            SoundTable.Dispose();
        }

        /// <summary>
        /// Creates a new loader for map hitsound.
        /// </summary>
        private IEnumerable<IExplicitPromise> CreateHitsoundLoaders()
        {
            foreach (var info in hitsoundInfos.Values)
            {
                if(string.IsNullOrEmpty(info.LookupName))
                    continue;

                var loader = new EffectAudioRequest(PathUtils.LocalRequestPath(info.File.FullName));
                loader.OnFinishedResult += (audio) => SoundTable.SetSound(info.LookupName, audio);
                yield return loader;
            }
        }

        /// <summary>
        /// Disposes hitsound loader if exists.
        /// </summary>
        private void DisposeHitsoundLoader()
        {
            if(hitsoundLoader != null)
                hitsoundLoader.Revoke();
            hitsoundLoader = null;
        }

        /// <summary>
        /// Finds all hitsounds used within the map.
        /// </summary>
        private void FindHitsounds()
        {
            Action<List<SoundInfo>> addSounds = (sounds) =>
            {
                for (int i = 0; i < sounds.Count; i++)
                {
                    foreach (var name in sounds[i].LookupNames)
                    {
                        // Skip if already added.
                        if (hitsoundInfos.ContainsKey(name))
                            break;

                        // Find hitsound file of current lookup name.
                        FileInfo file = FindAudio(name);
                        if (file != null)
                        {
                            hitsoundInfos[name] = new HitsoundLoadInfo()
                            {
                                File = file,
                                LookupName = name
                        };
                        }
                    }
                }
            };

            // Lookup all valid hit sounds from all hit objects.
            foreach (var hitObj in Map.HitObjects)
            {
                var curve = hitObj as IHasCurve;
                if (curve != null)
                {
                    for (int i = 0; i < curve.NodeSamples.Count; i++)
                        addSounds(curve.NodeSamples[i]);
                }
                for (int i = 0; i < hitObj.NestedObjects.Count; i++)
                    addSounds(hitObj.NestedObjects[i].Samples);
                addSounds(hitObj.Samples);
            }
        }

        private class HitsoundLoadInfo
        {
            /// <summary>
            /// The name associated with the hitsound.
            /// </summary>
            public string LookupName { get; set; }

            /// <summary>
            /// The file representing the hit sound.
            /// </summary>
            public FileInfo File { get; set; }

            /// <summary>
            /// The audio loaded from the map.
            /// </summary>
            IEffectAudio EffectAudio { get; set; }
        }
    }
}