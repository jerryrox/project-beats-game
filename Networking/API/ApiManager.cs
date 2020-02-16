using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.API.Osu;

namespace PBGame.Networking.API
{
    public class ApiManager : IApiManager {

        private Dictionary<ApiProviders, IApi> apis;


        public IOnlineUser OfflineUser { get; private set; } = new OfflineUser();


        public ApiManager()
        {
            apis = new Dictionary<ApiProviders, IApi>()
            {
                { ApiProviders.Osu, new OsuApi() }
            };
        }

        public IApi GetApi(ApiProviders provider) => apis[provider];
    }
}