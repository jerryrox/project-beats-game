using System;
using PBFramework.Networking.API;

namespace PBGame.Networking.API
{
    public abstract class BaseResponse : IApiResponse {

        protected readonly IHttpRequest request;


        public bool IsSuccess { get; protected set; }

        public string ErrorMessage { get; protected set; }


        protected BaseResponse(IHttpRequest request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));
            if(request.Response == null) throw new Exception("request does not contain a valid response.");

            this.request = request;
        }

        public virtual void Evaluate()
        {
            IsSuccess = request.Response.IsSuccess;
            ErrorMessage = request.Response.ErrorMessage;
        }

        public virtual void ApplyResponse(IApi api)
        {
            var headers = request?.Response?.Headers;
            if (headers != null)
            {
                // Set cookies
                foreach (var key in headers.Keys)
                {
                    if (key.Equals("set-cookie", StringComparison.OrdinalIgnoreCase))
                    {
                        api.Cookies.SetCookie(headers[key]);
                        break;
                    }
                }
            }
        }
    }
}