using System;
using PBGame.Stores;
using PBGame.Networking.API.Osu.Responses;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBGame.Networking.Maps;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Osu.Requests
{
    public class MapDownloadRequest : BaseRequest<IMapDownloadResponse>, IMapDownloadRequest {

        // public override string RequestTitle => 

        public IDownloadStore DownloadStore { get; set; }

        public OnlineMapset Mapset { get; set; }

        public bool IsNoVideo { get; set; }
        
        public override bool IsNotified => true;

        public override bool RequiresLogin => true;


        protected override IHttpRequest CreateRequest()
        {
            if (DownloadStore == null)
                throw new NullReferenceException(nameof(DownloadStore));

            var request = new HttpGetRequest(GenerateUrl(), 300, 0);
            if(IsNoVideo)
                request.AddQueryParam("noVideo", "1");
            return request;
        }

        protected override IMapDownloadResponse CreateResponse(IHttpRequest request) => new MapDownloadResponse(request, DownloadStore, Mapset.Id);

        /// <summary>
        /// Generates a download link for current mapset.
        /// Refer to this for more information.
        /// https://trello.com/c/xG4GbMNP/62-fix-download
        /// 
        /// If download breaks again, I need to keep digging into osu-web source for workaround.
        /// https://github.com/ppy/osu-web
        /// </summary>
        private string GenerateUrl()
        {
            // TODO: Temporarily use bloodcat for now.
            return $"https://bloodcat.com/osu/s/{Mapset.Id}";
            // return null;
        }
    }
}