using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;
using PBFramework.Networking.API;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Responses
{
    public class AuthResponse : ApiResponse {

        /// <summary>
        /// Returns the authentication information returned by the server.
        /// </summary>
        public Authentication Authentication { get; private set; }


        public AuthResponse(IHttpRequest request) : base(request)
        {
            
        }

        protected override void ParseResponseData(JToken responseData, IEventProgress progress)
        {
            var auth = responseData.ToObject<Authentication>();
            if (auth != null && !string.IsNullOrEmpty(auth.AccessToken))
            {
                Authentication = auth;
            }
            else
            {
                IsSuccess = false;
                ErrorMessage = "Access token is missing!";
            }
        }
    }
}