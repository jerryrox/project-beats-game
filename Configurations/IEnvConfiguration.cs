namespace PBGame.Configurations
{
    /// <summary>
    /// Configuration which holds environmental variables.
    /// </summary>
    public interface IEnvConfiguration {
        
        /// <summary>
        /// Returns whether the configuration is loaded for development environment.
        /// </summary>
        bool IsDevelopment { get; }

        /// <summary>
        /// Returns whether the config is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Environment variables currently loaded.
        /// </summary>
        EnvVariables Variables { get; }


        /// <summary>
        /// Loads environment values from the specified resource path.
        /// </summary>
        void Load(string path);
    }
}