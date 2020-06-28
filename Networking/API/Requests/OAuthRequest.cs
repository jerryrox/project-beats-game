using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.API.Responses;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Requests
{
    public class OAuthRequest : ApiRequest<OAuthResponse> {
    
        public OAuthRequest(IApi api, IApiProvider provider) : base(api, provider)
        {
    
        }

        protected override IHttpRequest CreateRequest() => new HttpPostRequest(api.GetUrl(provider, "/auth"));

        protected override OAuthResponse CreateResponse(IHttpRequest request) => new OAuthResponse(request);
    }
}