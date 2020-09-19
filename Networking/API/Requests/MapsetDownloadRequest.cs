using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Stores;
using PBGame.Networking.API.Responses;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Requests
{
    public class MapsetDownloadRequest : ApiRequest<MapsetDownloadResponse> {

        /// <summary>
        /// The store where the downloaded file will be stored to.
        /// </summary>
        public IDownloadStore DownloadStore { get; set; }

        /// <summary>
        /// The ID of the mapset to download.
        /// </summary>
        public string MapsetId { get; set; }


        public MapsetDownloadRequest(IApi api, IApiProvider provider) : base(api, provider)
        {
        }

        protected override IHttpRequest CreateRequest()
            => new HttpGetRequest(api.GetUrl(provider, "/mapsets/download"), 60 * 5);

        protected override MapsetDownloadResponse CreateResponse(IHttpRequest request)
            => new MapsetDownloadResponse(request.Response, DownloadStore, MapsetId);

        protected override void OnPreRequest()
        {
            InnerRequest.AddQueryParam("id", MapsetId);
        }
    }
}