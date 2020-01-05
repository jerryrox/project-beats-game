using PBGame.Rulesets.Maps;
using PBGame.Configurations.Maps;

namespace PBGame.Configurations
{
    public interface IMapConfiguration : IConfiguration {

        /// <summary>
        /// Returns the map config data for specified map.
        /// </summary>
        MapConfig GetConfig(IMap map);

        /// <summary>
        /// Returns the map config data for specified map hash.
        /// </summary>
        MapConfig GetConfig(string hash);

        /// <summary>
        /// Stores the specified configuration data to disk.
        /// </summary>
        void SetConfig(MapConfig config);
    }
}