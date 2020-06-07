using System;
using System.Linq;
using PBGame.IO;
using PBGame.Rulesets.Maps;
using PBGame.Configurations.Maps;
using PBFramework;
using PBFramework.DB;
using PBFramework.Debugging;

namespace PBGame.Configurations
{
    public class MapsetConfiguration : IMapsetConfiguration {

        private Database<MapsetConfig> database;


        public MapsetConfig GetConfig(IMapset mapset) => GetConfig(mapset.HashCode);

        public MapsetConfig GetConfig(int hash)
        {
            var hashString = hash.ToString();

            using (var result = database.Query()
                .Where(i => i["MapsetHash"].ToString() == hashString)
                .GetResult())
            {
                var config = result.FirstOrDefault();
                if (config == null)
                {
                    config = new MapsetConfig(hash);
                    config.InitializeAsNew();
                }
                return config;
            }
        }

        public void SetConfig(MapsetConfig config)
        {
            if(config == null) throw new ArgumentNullException(nameof(config));

            database.Edit().Write(config).Commit();
        }

        public void Load()
        {
            if(database != null) return;

            database = new Database<MapsetConfig>(GameDirectory.Configs.GetSubdirectory("mapsets"));
            if (!database.Initialize())
            {
                Logger.LogWarning("MapsetConfiguration.Load - Failed to initialize database");
            }
        }

        public void Save() => Logger.LogWarning("MapsetConfiguration.Save - Unimplemented interface method 'Save()' called. Use SetConfig instaed.");
    }
}