using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Stores.Parsers.Maps;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Threading;
using PBFramework;
using PBFramework.DB;
using PBFramework.Stores;
using PBFramework.Storages;
using PBFramework.Threading;
using PBFramework.Debugging;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.Stores
{
    public class MapsetStore : DirectoryBackedStore<Mapset>, IMapsetStore {

        private OsuMapsetParser osuParser;

        private List<Mapset> mapsets = new List<Mapset>();


        public IEnumerable<Mapset> Mapsets
        {
            get
            {
                lock (mapsets)
                {
                    return mapsets;
                }
            }
        }


        public MapsetStore(IModeManager modeManager)
        {
            osuParser = new OsuMapsetParser(modeManager);
        }

        public override Task Reload(TaskListener listener = null)
        {
            return Task.Run(async () =>
            {
                // Perform internal reloading routine.
                await base.Reload(listener?.CreateSubListener());

                // Retrieve processor count.

                // Load all mapsets from the storage.
                var rawMapsets = GetAll().ToList();
                var threadedLoader = new ThreadedLoader<Mapset, Mapset>(ProcessMapsetLoad);
                var results = await threadedLoader.StartLoad(
                    8,
                    rawMapsets,
                    listener: listener?.CreateSubListener<Mapset[]>()
                );

                // Add results to the mapsets list.
                lock (mapsets)
                {
                    mapsets.Clear();
                    for (int i = 0; i < results.Length; i++)
                    {
                        if (results[i] != null)
                            mapsets.Add(results[i]);
                    }
                }
                listener?.SetFinished();
            });
        }

        public Mapset Load(Guid id)
        {
            // Initialize
            InitModules(false);

            // Find entry with matching id.
            string stringId = id.ToString();
            Mapset mapset = null;
            using (var result = Database.Query()
                .Where(d => d["Id"].ToString().Equals(stringId, StringComparison.Ordinal))
                .GetResult())
            {
                Mapset rawMapset = result.FirstOrDefault();
                if(rawMapset != null)
                    mapset = LoadData(rawMapset);
            }

            return mapset;
        }

        public void DeleteMap(IOriginalMap map)
        {
            var file = map.Detail.MapFile;
            if (!file.Exists)
                throw new Exception($"Attempted to delete map that does not exist: {map.Metadata.Artist} - {map.Metadata.Title}");
            file.Delete();
            file.Refresh();
        }

        protected override IDatabase<Mapset> CreateDatabase() => new Database<Mapset>(GameDirectory.Maps.GetSubdirectory("data"));

        protected override IDirectoryStorage CreateStorage() => new DirectoryStorage(GameDirectory.Maps.GetSubdirectory("files"));

        protected override Mapset ParseData(DirectoryInfo directory, Mapset mapset)
        {
            if (mapset == null)
            {
                mapset = new Mapset();
                mapset.ImportedDate = DateTime.Now;
            }

            // TODO: Support for other data formats.
            try
            {
                return osuParser.Parse(directory, mapset);
            }
            catch (Exception e)
            {
                Logger.LogError($"MapsetStore.ParseData - Error while parsing mapset: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// Processes the specified input mapset to make it ready for in-game use.
        /// </summary>
        private Mapset ProcessMapsetLoad(Mapset input)
        {
            return LoadData(input);
        }
    }
}