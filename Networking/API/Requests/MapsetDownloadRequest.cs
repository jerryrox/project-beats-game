using System.Collections.Generic;
using PBGame.Stores;
using PBGame.Networking.API.Responses;
using PBGame.Networking.Maps;
using PBGame.Notifications;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Requests
{
    public class MapsetDownloadRequest : ApiRequest<MapsetDownloadResponse> {

        /// <summary>
        /// The store where the downloaded file will be stored to.
        /// </summary>
        public IDownloadStore DownloadStore { get; set; }

        /// <summary>
        /// The mapset to download.
        /// </summary>
        public OnlineMapset Mapset { get; set; }


        public MapsetDownloadRequest(IApi api, IApiProvider provider) : base(api, provider)
        {
        }

        public override Notification CreateNotification()
        {
            Notification notification = new Notification()
            {
                CoverImage = Mapset.CoverImage,
                Id = Mapset.Id.ToString(),
                Message = $"Downloading {Mapset.Artist} - {Mapset.Title} ({Mapset.Creator})",
                Scope = NotificationScope.Stored,
                Type = NotificationType.Verbose,
                Actions = new List<NotificationAction>()
                {
                    new NotificationAction()
                    {
                        Name = "Stop download",
                        Action = Dispose,
                        ShouldDismiss = true,
                    }
                },
            };
            return notification;
        }

        protected override IHttpRequest CreateRequest()
            => new HttpGetRequest(api.GetUrl(provider, "MapsetDownload"), 60 * 5);

        protected override MapsetDownloadResponse CreateResponse(IHttpRequest request)
            => new MapsetDownloadResponse(request.Response, DownloadStore, Mapset.Id.ToString());

        protected override void OnPreRequest()
        {
            InnerRequest.AddQueryParam("id", Mapset.Id.ToString());
        }
    }
}