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
    public class MapConfiguration : IMapConfiguration {

        private Database<MapConfig> database;


        public MapConfig GetConfig(IMap map) => GetConfig(map.Detail.Hash);

        public MapConfig GetConfig(string hash)
        {
            using (var result = database.Query()
                .Where(i => i["MapHash"].ToString() == hash)
                .GetResult())
            {
                var config = result.FirstOrDefault();
                if (config == null)
                {
                    config = new MapConfig(hash);
                    config.InitializeAsNew();
                    SetConfig(config);
                }
                return config;
            }
        }

        public void SetConfig(MapConfig config)
        {
            if(config == null) throw new ArgumentNullException(nameof(config));
            if(string.IsNullOrEmpty(config.MapHash)) throw new ArgumentException("The specified config does not contain a valid map hash!");

            database.Edit().Write(config).Commit();
        }

        public void Load()
        {
            if(database != null) return;

            database = new Database<MapConfig>(GameDirectory.Configs.GetSubdirectory("maps"));
            if (!database.Initialize())
            {
                Logger.LogWarning("MapConfiguration.Load - Failed to initialize database");
            }
        }

        public void Save() => Logger.LogWarning("MapConfiguration.Save - Unimplemented interface method 'Save()' called. Use SetConfig instaed.");
    }
}