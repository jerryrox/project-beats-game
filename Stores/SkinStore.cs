using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Skins;
using PBGame.Stores.Parsers.Skins;
using PBFramework;
using PBFramework.DB;
using PBFramework.Stores;
using PBFramework.Storages;
using PBFramework.Threading;

namespace PBGame.Stores
{
    public class SkinStore : DirectoryBackedStore<Skin>, ISkinStore {

        private BeatsSkinParser beatsParser;

        private List<Skin> skins = new List<Skin>();


        public IEnumerable<Skin> Skins
        {
            get
            {
                lock (skins)
                {
                    return skins;
                }
            }
        }


        public override async Task Reload(ISimpleProgress progress)
        {
            // Wait for internal processing
            await base.Reload(progress);

            // Load all skins from the storage.
            lock(skins)
            {
                skins.Clear();
                using(var results = Database.Query().Preload().GetResult())
                {
                    skins.AddRange(results);
                }
            }
        }

        protected override IDatabase<Skin> CreateDatabase() => new Database<Skin>(GameDirectory.Skins.GetSubdirectory("data"));

        protected override IDirectoryStorage CreateStorage() => new DirectoryStorage(GameDirectory.Skins.GetSubdirectory("files"));

        protected override Skin ParseData(DirectoryInfo directory)
        {
            // TODO: Support for other data formats.
            return beatsParser.Parse(directory);
        }
    }
}