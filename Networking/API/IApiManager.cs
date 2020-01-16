namespace PBGame.Networking.API
{
    public interface IApiManager {

        /// <summary>
        /// Returns the API instance for the specified provider type.
        /// </summary>
        IApi GetApi(ApiProviders provider);
    }
}