using System;
using PBGame.Stores;
using PBGame.Networking.API.Osu.Responses;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Osu.Requests
{
    public class MapDownloadRequest : BaseRequest<MapDownloadResponse> {

        private IDownloadStore downloadStore;
        private int mapsetId;
        private bool noVideo;


        public override bool IsNotified => true;

        // public override string RequestTitle => 


        public MapDownloadRequest(IDownloadStore downloadStore, int mapsetId, bool noVideo = false)
        {
            if(downloadStore == null) throw new ArgumentNullException(nameof(downloadStore));

            this.downloadStore = downloadStore;
            this.mapsetId = mapsetId;
            this.noVideo = noVideo;
        }

        protected override IHttpRequest CreateRequest()
        {
            var request = new HttpGetRequest(Api.GetUrl($"beatmapsets/{mapsetId}/download"), 300, 0);
            if(noVideo)
                request.AddQueryParam("noVideo", "1");
            return request;
        }

        protected override MapDownloadResponse CreateResponse(IHttpRequest request) => new MapDownloadResponse(request, downloadStore, mapsetId);
    }
}