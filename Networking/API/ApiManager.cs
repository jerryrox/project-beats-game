using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Networking.API.Osu;

namespace PBGame.Networking.API
{
    public class ApiManager : IApiManager {

        private Dictionary<ApiProviderType, IApi> apis;


        public IOnlineUser OfflineUser { get; private set; } = new OfflineUser();


        public ApiManager()
        {
            apis = new Dictionary<ApiProviderType, IApi>()
            {
                { ApiProviderType.Osu, new OsuApi() }
            };
        }

        public IApi GetApi(ApiProviderType provider) => apis[provider];

        public IApi GetRelevantApi(IMap map) => apis.Values.FirstOrDefault(api => api.IsRelevantMap(map));

        public IEnumerable<IApi> GetAllApi() => apis.Values;
    }
}