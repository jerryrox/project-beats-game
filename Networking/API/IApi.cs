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
        ApiProviders ApiType { get; }

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
    }
}