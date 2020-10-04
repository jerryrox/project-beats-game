using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBFramework.Data.Bindables;

namespace PBGame.Networking.API
{
    /// <summary>
    /// Networking bridge between PB Game and PB Api server.
    /// </summary>
    public interface IApi {

        /// <summary>
        /// Returns the offline API user instance.
        /// </summary>
        IOnlineUser OfflineUser { get; }

        /// <summary>
        /// Returns the current online user.
        /// </summary>
        IReadOnlyBindable<IOnlineUser> User { get; }

        /// <summary>
        /// Returns the current authentication session.
        /// </summary>
        IReadOnlyBindable<Authentication> Authentication { get; }

        /// <summary>
        /// Returns whether the user is currently logged in.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Returns the provider instance which the user is currently authenticated for.
        /// </summary>
        IApiProvider AuthenticatedProvider { get; }


        /// <summary>
        /// Returns the provider instance for the specified type.
        /// </summary>
        IApiProvider GetProvider(ApiProviderType type);

        /// <summary>
        /// Returns the full endpoint url using the specified provider type and path.
        /// </summary>
        string GetUrl(IApiProvider type, string path);

        /// <summary>
        /// Logs the user out of current online session.
        /// </summary>
        void Logout();

        /// <summary>
        /// Processes the specified request under monitoring of the API.
        /// </summary>
        void Request(IApiRequest request);

        /// <summary>
        /// Handles additional actions based on the given response.
        /// </summary>
        void HandleResponse(IApiResponse response);
    }
}