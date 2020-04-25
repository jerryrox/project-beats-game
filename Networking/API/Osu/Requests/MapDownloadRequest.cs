using System;
using PBGame.Stores;
using PBGame.Networking.API.Osu.Responses;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Osu.Requests
{
    public class MapDownloadRequest : BaseRequest<IMapDownloadResponse>, IMapDownloadRequest {

        public override bool IsNotified => true;

        // public override string RequestTitle => 

        public IDownloadStore DownloadStore { get; set; }

        public int MapsetId { get; set; }

        public bool IsNoVideo { get; set; }


        protected override IHttpRequest CreateRequest()
        {
            if (DownloadStore == null)
                throw new NullReferenceException(nameof(DownloadStore));

            var request = new HttpGetRequest(Api.GetUrl($"beatmapsets/{MapsetId}/download"), 300, 0);
            if(IsNoVideo)
                request.AddQueryParam("noVideo", "1");
            return request;
        }

        protected override IMapDownloadResponse CreateResponse(IHttpRequest request) => new MapDownloadResponse(request, DownloadStore, MapsetId);
    }
}