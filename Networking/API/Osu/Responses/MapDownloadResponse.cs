using System;
using PBGame.Stores;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Osu.Responses
{
    public class MapDownloadResponse : BaseResponse {

        private IDownloadStore downloadStore;
        private int mapsetId;


        public MapDownloadResponse(IHttpRequest request, IDownloadStore downloadStore, int mapsetId) : base(request)
        {
            if(downloadStore == null) throw new ArgumentNullException(nameof(downloadStore));

            this.downloadStore = downloadStore;
            this.mapsetId = mapsetId;
        }

        public override void Evaluate()
        {
            IsSuccess = request.Response.Code == 200;
            ErrorMessage = request.Response.ErrorMessage;

            if (IsSuccess)
            {
                var bytes = request.Response.ByteData;
                if(bytes.Length > 0)
                    downloadStore.MapStorage.Write(GetFileName(), bytes);
                else
                {
                    IsSuccess = false;
                    ErrorMessage = "Missing byte data.";
                }
            }
        }

        /// <summary>
        /// Returns the file name which the map will be saved as.
        /// </summary>
        private string GetFileName() => $"{mapsetId}.osz";
    }
}