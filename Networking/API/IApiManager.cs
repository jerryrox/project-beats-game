using System.Collections.Generic;
using PBGame.Rulesets.Maps;

namespace PBGame.Networking.API
{
    public interface IApiManager {

        /// <summary>
        /// Returns the instance of an offline user.
        /// </summary>
        IOnlineUser OfflineUser { get; }


        /// <summary>
        /// Returns the API instance for the specified provider type.
        /// </summary>
        IApi GetApi(ApiProviders provider);

        /// <summary>
        /// Returns the relevant API for the specified map.
        /// </summary>
        IApi GetRelevantApi(IMap map);

        /// <summary>
        /// Returns all apis supported.
        /// </summary>
        IEnumerable<IApi> GetAllApi();
    }
}