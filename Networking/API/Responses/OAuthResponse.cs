using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;
using PBFramework.Networking.API;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Responses
{
    public class OAuthResponse : ApiResponse {

        /// <summary>
        /// Returns the url to OAuth page.
        /// </summary>
        public string OAuthUrl { get; private set; }


        public OAuthResponse(IHttpRequest request) : base(request)
        {
    
        }

        protected override void ParseResponseData(JToken responseData)
        {
            OAuthUrl = responseData["oauthUrl"].ToString();
            if (string.IsNullOrEmpty(OAuthUrl))
            {
                EvaluateFail("OAuth URL is missing!");
            }
            else
            {
                EvaluateSuccess();
            }
        }
    }
}