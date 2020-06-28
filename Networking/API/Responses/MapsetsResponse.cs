using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.Maps;
using PBFramework.Threading;
using PBFramework.Networking.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Responses
{
    public class MapsetsResponse : ApiResponse {

        /// <summary>
        /// Returns the array of all mapsets retrieved in this query.
        /// </summary>
        public OnlineMapset[] Mapsets { get; private set; }

        /// <summary>
        /// Returns the cursor of current search for retrieval of the next set of results.
        /// </summary>
        public string Cursor { get; private set; }

        /// <summary>
        /// Returns the total number of mapsets retrievable by the provider.
        /// </summary>
        public int? Total { get; private set; }


        public MapsetsResponse(IHttpRequest request) : base(request) {}

        protected override void ParseResponseData(JToken responseData, IEventProgress progress)
        {
            var data = responseData.ToObject<JObject>();
            Mapsets = data["mapsets"].ToObject<OnlineMapset[]>();
            if(data.ContainsKey("cursor"))
                Cursor = data["cursor"].ToString();
            if(data.ContainsKey("total"))
                Total = data["total"].Value<int>();
        }
    }
}