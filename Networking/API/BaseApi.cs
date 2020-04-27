using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Notifications;
using PBFramework.Data.Bindables;
using PBFramework.Networking;

namespace PBGame.Networking.API
{
    public abstract class BaseApi : IApi
    {
        protected readonly OfflineUser offlineUser = new OfflineUser();


        public abstract string BaseUrl { get; }

        public abstract ApiProviders ApiType { get; }

        public abstract string Name { get; }

        public abstract string IconName { get; }

        public abstract IApiAdaptor Adaptor { get; }

        public abstract IRequestFactory RequestFactory { get; }

        public BindableBool IsOnline { get; private set; } = new BindableBool(false);

        public Bindable<IOnlineUser> User { get; protected set; }

        public CookieContainer Cookies { get; private set; } = new CookieContainer();

        public INotificationBox NotificationBox { get; set; }


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
                    NotificationBox?.Add(new Notification() {
                        Message = $"Failed API request. ({response.ErrorMessage})",
                        Type = NotificationType.Negative,
                    });
                }
                request.Dispose();
            };

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