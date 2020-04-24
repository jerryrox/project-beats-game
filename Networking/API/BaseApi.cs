using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBFramework.Data.Bindables;
using PBFramework.Networking;

namespace PBGame.Networking.API
{
    public abstract class BaseApi : IApi {

        protected readonly OfflineUser offlineUser = new OfflineUser();


        public abstract string BaseUrl { get; }

        public abstract ApiProviders ApiType { get; }

        public abstract string Name { get; }

        public abstract string IconName { get; }

        public abstract IApiAdaptor Adaptor { get; }

        public BindableBool IsOnline { get; private set; } = new BindableBool(false);

        public Bindable<IOnlineUser> User { get; protected set; }

        public CookieContainer Cookies { get; private set; } = new CookieContainer();


        public BaseApi()
        {
            User = new Bindable<IOnlineUser>(offlineUser);

            User.OnValueChanged += (user, _) => IsOnline.Value = user != offlineUser;
        }

        public string GetUrl(string path)
        {
            if(path.StartsWith("/"))
                return BaseUrl + path;
            return $"{BaseUrl}/{path}";
        }

        public void Request(IApiRequest request)
        {
            // Prepare for request.
            request.Prepare(this);

            // Setup callback
            var promise = request.Promise;
            request.OnRequestEnd += (response) =>
            {
                if (response.IsSuccess)
                {
                    // Retrieve the response and let response handle state changes.
                    request.Response.ApplyResponse(this);
                }
                else
                {
                    // TODO: Display failed on notification.
                }
                request.Dispose();
            };

            // TODO: Notification
            if (request.IsNotified)
            {
            }

            // Start requesting
            request.Request();
        }

        public virtual void Logout()
        {
            User.Value = offlineUser;
        }

        public abstract IEnumerable<GameModes> GetGameModes();

        public bool IsRelevantMode(GameModes gameMode) => GetGameModes().Any(m => m == gameMode);

        public bool IsRelevantMap(IMap map) => map == null ? false : IsRelevantMode(map.Detail.GameMode);
    }
}