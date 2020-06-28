using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Stores;
using PBGame.Rulesets;
using PBGame.Networking.API.Responses;
using PBGame.Networking.Maps;
using PBFramework.Networking.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Requests
{
    public class MapsetsRequest : ApiRequest<MapsetsResponse>
    {

        /// <summary>
        /// The cursor for the next set of mapsets.
        /// </summary>
        public string Cursor { get; set; }

        /// <summary>
        /// The game mode to search for.
        /// </summary>
        public GameModeType GameMode { get; set; }

        /// <summary>
        /// The category to search for.
        /// </summary>
        public MapCategoryType Category { get; set; }

        /// <summary>
        /// The genre to search for.
        /// </summary>
        public MapGenreType Genre { get; set; }

        /// <summary>
        /// The language to search for.
        /// </summary>
        public MapLanguageType Language { get; set; }

        /// <summary>
        /// The map sorting column.
        /// </summary>
        public MapSortType Sort { get; set; }

        /// <summary>
        /// The query string to search mapsets with.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Whethere the mapset must have a video.
        /// </summary>
        public bool HasVideo { get; set; }

        /// <summary>
        /// Whether the mapset must have a storyboard.
        /// </summary>
        public bool HasStoryboard { get; set; }

        /// <summary>
        /// Whether the mapset results should be in descending order.
        /// </summary>
        public bool IsDescending { get; set; }


        public MapsetsRequest(IApi api, IApiProvider provider) : base(api, provider)
        {
        }

        protected override IHttpRequest CreateRequest()
        {
            var request = new HttpGetRequest(api.GetUrl(provider, $"/mapsets"));
            request.AddQueryParams(GetQueries());
            return request;
        }

        protected override MapsetsResponse CreateResponse(IHttpRequest request) => new MapsetsResponse(request);

        /// <summary>
        /// Returns all query parameters.
        /// </summary>
        private IEnumerable<KeyValuePair<string, string>> GetQueries()
        {
            if (!string.IsNullOrEmpty(this.Cursor))
            {
                var cursor = JsonConvert.DeserializeObject<JObject>(this.Cursor);
                foreach (var pair in cursor)
                    yield return new KeyValuePair<string, string>(pair.Key, pair.Value.ToString());
            }
            yield return new KeyValuePair<string, string>("mode", ((int)GameMode).ToString());
            yield return new KeyValuePair<string, string>("category", ((int)Category).ToString());
            yield return new KeyValuePair<string, string>("genre", ((int)Genre).ToString());
            yield return new KeyValuePair<string, string>("language", ((int)Language).ToString());
            yield return new KeyValuePair<string, string>("sort", ((int)Sort).ToString());
            yield return new KeyValuePair<string, string>("query", Query);
            yield return new KeyValuePair<string, string>("hasVideo", HasVideo.ToString().ToLower());
            yield return new KeyValuePair<string, string>("hasStoryboard", HasStoryboard.ToString().ToLower());
            yield return new KeyValuePair<string, string>("isDescending", IsDescending.ToString().ToLower());
        }
    }
}