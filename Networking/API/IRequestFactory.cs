using PBGame.Networking.API.Requests;

namespace PBGame.Networking.API
{
    /// <summary>
    /// Interface designed to return generalized reference of API requests as interfaces.
    /// </summary>
    public interface IRequestFactory {

        /// <summary>
        /// Returns a new login request.
        /// </summary>
        ILoginRequest GetLogin();

        /// <summary>
        /// Returns a new map download request.
        /// </summary>
        IMapDownloadRequest GetMapDownload();

        /// <summary>
        /// Returns a new mapset list request.
        /// </summary>
        IMapsetListRequest GetMapsetList();
    }
}