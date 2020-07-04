using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;

namespace PBGame.Networking.API
{
    public abstract class ApiProvider : IApiProvider {

        protected IApi api;


        public abstract ApiProviderType Type { get; }

        public virtual bool IsOAuthLogin => false;

        public abstract string Name { get; }

        public virtual string InternalName => Type.ToString();

        public virtual string IconName => $"icon-provider-{InternalName}";


        protected ApiProvider(IApi api)
        {
            if(api == null)
                throw new ArgumentNullException(nameof(api));

            this.api = api;
        }

        public virtual AuthRequest Auth() => CreateRequest(new AuthRequest(api, this));

        public virtual OAuthRequest OAuth() => CreateRequest(new OAuthRequest(api, this));

        public virtual MeRequest Me() => CreateRequest(new MeRequest(api, this));

        public virtual MapsetsRequest Mapsets() => CreateRequest(new MapsetsRequest(api, this));

        public virtual MapsetDownloadRequest MapsetDownload() => CreateRequest(new MapsetDownloadRequest(api, this));

        /// <summary>
        /// Returns the specified request after hooking response to API's response handler.
        /// </summary>
        protected T CreateRequest<T>(T request)
            where T : IApiRequest
        {
            request.RawResponse.OnNewRawValue += (response) =>
            {
                if(response is IApiResponse apiResponse)
                    api.HandleResponse(apiResponse);
            };
            return request;
        }
    }
}