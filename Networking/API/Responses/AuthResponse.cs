using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;
using PBFramework.Networking;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Responses
{
    public class AuthResponse : ApiResponse {

        /// <summary>
        /// Returns the authentication information returned by the server.
        /// </summary>
        public Authentication Authentication { get; private set; }


        public AuthResponse(IWebResponse response) : base(response)
        {
            
        }

        protected override void ParseResponseData(JToken responseData)
        {
            var auth = responseData.ToObject<Authentication>();
            if (auth != null && !string.IsNullOrEmpty(auth.AccessToken))
            {
                Authentication = auth;
                EvaluateSuccess();
            }
            else
            {
                EvaluateFail("Access token is missing!");
            }
        }
    }
}