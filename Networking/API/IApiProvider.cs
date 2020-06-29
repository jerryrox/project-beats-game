using PBGame.Networking.API.Requests;

namespace PBGame.Networking.API
{
    public interface IApiProvider {

        /// <summary>
        /// Returns the type of the api provider.
        /// </summary>
        ApiProviderType Type { get; }

        /// <summary>
        /// Returns whether this provider uses OAuth for logging in.
        /// </summary>
        bool IsOAuthLogin { get; }

        /// <summary>
        /// Returns the displayed name of the provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the internal name of the provider used within the system.
        /// </summary>
        string InternalName { get; }

        /// <summary>
        /// Returns the name of the icon that represents this provider.
        /// </summary>
        string IconName { get; }


        /// <summary>
        /// Creates a new authentication request.
        /// </summary>
        AuthRequest Auth();

        /// <summary>
        /// Creates a new OAuth request.
        /// </summary>
        OAuthRequest OAuth();

        /// <summary>
        /// Creates a new me request.
        /// </summary>
        MeRequest Me();

        /// <summary>
        /// Creates a new mapsets search request.
        /// </summary>
        MapsetsRequest Mapsets();

        /// <summary>
        /// Creates a new mapset download request.
        /// </summary>
        MapsetDownloadRequest MapsetDownload();
    }
}