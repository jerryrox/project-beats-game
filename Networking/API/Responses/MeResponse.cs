using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;
using PBFramework.Networking;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Responses
{
    public class MeResponse : ApiResponse {

        private IApiProvider provider;


        /// <summary>
        /// Returns the online user information retrieved from the server.
        /// </summary>
        public OnlineUser User { get; private set; }


        public MeResponse(IWebResponse response, IApiProvider provider) : base(response)
        {
            this.provider = provider;
        }

        protected override void ParseResponseData(JToken responseData)
        {
            var user = responseData.ToObject<OnlineUser>();
            if (user == null)
            {
                EvaluateFail("Failed to parse user information.");
            }
            else if (string.IsNullOrEmpty(user.Id))
            {
                EvaluateFail("An invalid user Id was given.");
            }
            else if (string.IsNullOrEmpty(user.Username))
            {
                EvaluateFail("An invalid username was given.");
            }
            else
            {
                this.User = user;
                this.User.Provider = provider;
                EvaluateSuccess();
            }
        }
    }
}