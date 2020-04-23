using System;
using PBGame.Networking.API.Osu.Responses;
using PBGame.Networking.Maps;
using PBFramework.Services;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Osu.Requests
{
    public class MapsetListRequest : BaseRequest<MapsetListResponse> {

        public int? CursorDate { get; set; } = null;

        public int? CursorId { get; set; } = null;

        public int? Mode { get; set; } = null;

        public MapCategories Category { get; set; } = MapCategories.Any;

        public MapGenres Genre { get; set; } = MapGenres.Any;

        public MapLanguages Language { get; set; } = MapLanguages.Any;

        public MapSortType Sort { get; set; } = MapSortType.Ranked;

        public bool IsDescending { get; set; } = true;

        public bool HasVideo { get; set; } = false;

        public bool HasStoryboard { get; set; } = false;

        public string SearchTerm { get; set; } = null;


        protected override IHttpRequest CreateRequest()
        {
            var request = new HttpGetRequest(Api.GetUrl("beatmapsets/search"));
            if(CursorDate.HasValue)
                request.AddQueryParam("cursor[approved_date]", CursorDate.Value.ToString());
            if(CursorId.HasValue)
                request.AddQueryParam("cursor[_id]", CursorId.Value.ToString());
            if(Mode.HasValue)
                request.AddQueryParam("m", Mode.Value.ToString());
            if(Category != MapCategories.Any)
                request.AddQueryParam("s", Category.ToString().ToLower());
            if(Genre != MapGenres.Any)
                request.AddQueryParam("g", ((int)Genre).ToString());
            if(Language != MapLanguages.Any)
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

        protected override MapsetListResponse CreateResponse(IHttpRequest request) => new MapsetListResponse(request, OnMapsetsParsed);

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