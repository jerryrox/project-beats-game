using PBGame.Networking.API.Responses;
using PBGame.Stores;

namespace PBGame.Networking.API.Requests
{
    public interface IMapDownloadRequest : IApiRequest<IMapDownloadResponse> {
    
        /// <summary>
        /// The target store to download the map to.
        /// </summary>
        IDownloadStore DownloadStore { get; set; }

        /// <summary>
        /// Id of the mapset to download.
        /// </summary>
        int MapsetId { get; set; }

        /// <summary>
        /// Whether video should be included if the mapset contains it.
        /// </summary>
        bool IsNoVideo { get; set; }
    }
}