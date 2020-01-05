namespace PBGame.Configurations
{
    /// <summary>
    /// Provides the basic functionalities of a configuration provider.
    /// </summary>
    public interface IConfiguration {

        /// <summary>
        /// Loads the configuration from data source.
        /// </summary>
        void Load();

        /// <summary>
        /// Saves the configuration to destination.
        /// </summary>
        void Save();
    }
}