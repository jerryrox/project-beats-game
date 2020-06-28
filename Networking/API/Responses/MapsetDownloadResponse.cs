using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Stores;
using PBFramework.Threading;
using PBFramework.Networking;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Responses
{
    public class MapsetDownloadResponse : ApiResponse {

        private IDownloadStore downloadStore;
        private string mapsetId;


        public MapsetDownloadResponse(IHttpRequest request, IDownloadStore downloadStore, string mapsetId) : base(request)
        {
            this.downloadStore = downloadStore;
            this.mapsetId = mapsetId;
        }

        protected override void ParseResponse(IWebResponse response)
        {
            if (response.Code == 200)
            {
                var bytes = response.ByteData;
                if (bytes != null && bytes.Length > 0)
                {
                    downloadStore.MapStorage.Write(GetFileName(), bytes);
                    EvaluateSuccess();
                }
                else
                {
                    EvaluateFail("Missing byte data.");
                }
            }
            else
            {
                EvaluateFail(response.ErrorMessage ?? "Download failed due to unknown reason.");
            }
        }

        /// <summary>
        /// Returns the file name which the mapset will be saved as.
        /// </summary>
        private string GetFileName() => $"{mapsetId}.zip";
    }
}