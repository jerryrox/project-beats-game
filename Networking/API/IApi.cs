using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Notifications;
using PBFramework.Data.Bindables;
using PBFramework.Networking;

namespace PBGame.Networking.API
{
    public interface IApi {

        /// <summary>
        /// Returns the base url used for requests.
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// Returns the provider type of this api.
        /// </summary>
        ApiProviderType ApiType { get; }

        /// <summary>
        /// The displayed name of the API provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the spritename of the icon representing the API.
        /// </summary>
        string IconName { get; }

        /// <summary>
        /// Returns the adaptor for this api.
        /// </summary>
        IApiAdaptor Adaptor { get; }

        /// <summary>
        /// Returns the request factory of the api.
        /// </summary>
        IRequestFactory RequestFactory { get; }

        /// <summary>
        /// Returns whether the user is currently logged into the API server.
        /// </summary>
        BindableBool IsOnline { get; }

        /// <summary>
        /// Returns the user logged into the API server.
        /// </summary>
        Bindable<IOnlineUser> User { get; }

        /// <summary>
        /// Returns the cookie container instance.
        /// </summary>
        CookieContainer Cookies { get; }

        /// <summary>
        /// An optional notification box for notifying certain events happening in API.
        /// </summary>
        INotificationBox NotificationBox { get; set; }


        /// <summary>
        /// Returns the complete url for the specified API path.
        /// </summary>
        string GetUrl(string path);

        /// <summary>
        /// Starts the specified request.
        /// </summary>
        void Request(IApiRequest request);

        /// <summary>
        /// Logs out from current online session.
        /// </summary>
        void Logout();

        /// <summary>
        /// Returns all game modes which is linked to the provider of this api.
        /// </summary>
        IEnumerable<GameModeType> GetGameModes();

        /// <summary>
        /// Returns whether the specified game mode is relevant to this api's provider.
        /// </summary>
        bool IsRelevantMode(GameModeType gameMode);

        /// <summary>
        /// Returns whether the specified map is relevant to this api's provider.
        /// </summary>
        bool IsRelevantMap(IMap map);
    }
}