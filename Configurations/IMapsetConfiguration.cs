using PBGame.Rulesets.Maps;
using PBGame.Configurations.Maps;

namespace PBGame.Configurations
{
    public interface IMapsetConfiguration : IConfiguration {

        /// <summary>
        /// Returns the mapset config data for specified mapset.
        /// </summary>
        MapsetConfig GetConfig(IMapset mapset);

        /// <summary>
        /// Returns the mapset config data for specified mapset hash.
        /// </summary>
        MapsetConfig GetConfig(int hash);

        /// <summary>
        /// Stores the specified configuration data to disk.
        /// </summary>
        void SetConfig(MapsetConfig config);
    }
}