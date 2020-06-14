using PBGame.Networking.API.Responses;
using PBGame.Networking.Maps;
using PBGame.Stores;

namespace PBGame.Networking.API.Requests
{
    public interface IMapDownloadRequest : IApiRequest<IMapDownloadResponse> {
    
        /// <summary>
        /// The target store to download the map to.
        /// </summary>
        IDownloadStore DownloadStore { get; set; }

        /// <summary>
        /// The mapset to download.
        /// </summary>
        OnlineMapset Mapset { get; set; }

        /// <summary>
        /// Whether video should be included if the mapset contains it.
        /// </summary>
        bool IsNoVideo { get; set; }
    }
}