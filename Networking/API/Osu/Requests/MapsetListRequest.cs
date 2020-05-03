using System;
using PBGame.Networking.API.Osu.Responses;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBGame.Networking.Maps;
using PBFramework.Services;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Osu.Requests
{
    public class MapsetListRequest : BaseRequest<IMapsetListResponse>, IMapsetListRequest {

        public string CursorName { get; set; } = "approved_date";

        public string CursorValue { get; set; } = null;

        public int? CursorId { get; set; } = null;

        public int? Mode { get; set; } = null;

        public MapCategoryType Category { get; set; } = MapCategoryType.Any;

        public MapGenreType Genre { get; set; } = MapGenreType.Any;

        public MapLanguageType Language { get; set; } = MapLanguageType.Any;

        public MapSortType Sort { get; set; } = MapSortType.Ranked;

        public bool IsDescending { get; set; } = true;

        public bool HasVideo { get; set; } = false;

        public bool HasStoryboard { get; set; } = false;

        public string SearchTerm { get; set; } = null;

        public override bool RequiresLogin => false;


        protected override IHttpRequest CreateRequest()
        {
            var request = new HttpGetRequest(Api.GetUrl("beatmapsets/search"));
            if(CursorValue != null && CursorName != null)
                request.AddQueryParam($"cursor[{CursorName}]", CursorValue);
            if(CursorId.HasValue)
                request.AddQueryParam("cursor[_id]", CursorId.Value.ToString());
            if(Mode.HasValue)
                request.AddQueryParam("m", Mode.Value.ToString());
            if(Category != MapCategoryType.Any)
                request.AddQueryParam("s", Category.ToString().ToLower());
            if(Genre != MapGenreType.Any)
                request.AddQueryParam("g", ((int)Genre).ToString());
            if(Language != MapLanguageType.Any)
                request.AddQueryParam("l", ((int)Language).ToString());
            if(!string.IsNullOrWhiteSpace(SearchTerm))
                request.AddQueryParam("q", SearchTerm.Trim());
            if(Sort != MapSortType.Ranked || !IsDescending)
                request.AddQueryParam("sort", Api.Adaptor.GetMapSortName(Sort, IsDescending));
            if(HasVideo && HasStoryboard)
                request.AddQueryParam("e", "storyboard.video");
            else if(HasVideo)
                request.AddQueryParam("e", "video");
            else if(HasStoryboard)
                request.AddQueryParam("e", "storyboard");
            return request;
        }

        protected override IMapsetListResponse CreateResponse(IHttpRequest request) => new MapsetListResponse(request, OnMapsetsParsed);

        protected override void OnHttpResponse()
        {
            // Let the response start parsing the mapsets in another thread.
            // Once finished, we then fire the request end event to listeners.
            Response.Evaluate();
        }

        /// <summary>
        /// Invokes the mapsets parsed event to the listeners.
        /// </summary>
        private void OnMapsetsParsed()
        {
            // Make sure it's called on the main thread.
            UnityThreadService.Dispatch(() =>
            {
                InvokeRequestEnd();
                return null;
            });
        }
    }
}