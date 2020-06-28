using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.API.Responses;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Requests
{
    public class AuthRequest : ApiRequest<AuthResponse> {

        /// <summary>
        /// Username credential.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password credential.
        /// </summary>
        public string Password { get; set; }


        public AuthRequest(IApi api, IApiProvider provider) : base(api, provider)
        {
    
        }

        protected override IHttpRequest CreateRequest()
        {
            var request = new HttpPostRequest(api.GetUrl(provider, "/auth"));
            var postData = new FormPostData();
            postData.AddField("username", Username ?? "");
            postData.AddField("password", Password ?? "");
            request.SetPostData(postData);
            return request;
        }

        protected override AuthResponse CreateResponse(IHttpRequest request) => new AuthResponse(request);
    }
}