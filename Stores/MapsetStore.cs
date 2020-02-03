using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Stores.Parsers.Maps;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBFramework;
using PBFramework.DB;
using PBFramework.Stores;
using PBFramework.Storages;
using PBFramework.Threading;
using PBFramework.Debugging;

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

        public override async Task Reload(ISimpleProgress progress)
        {
            // Perform internal reloading routine.
            await base.Reload(progress);

            // Load all mapsets from the storage.
            lock (mapsets)
            {
                mapsets.Clear();
                foreach (var mapset in GetAll())
                {
                    var loadedMapset = LoadData(mapset);
                    if(loadedMapset != null)
                        mapsets.Add(loadedMapset);
                }
            }
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
    }
}