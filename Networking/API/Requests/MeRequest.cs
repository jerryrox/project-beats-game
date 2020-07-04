using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.API.Responses;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Requests
{
    public class MeRequest : ApiRequest<MeResponse> {
    
        public MeRequest(IApi api, IApiProvider provider) : base(api, provider)
        {
        }

        protected override IHttpRequest CreateRequest() => new HttpGetRequest(api.GetUrl(provider, "/me"));

        protected override MeResponse CreateResponse(IHttpRequest request) => new MeResponse(request.Response, provider);
    }
}