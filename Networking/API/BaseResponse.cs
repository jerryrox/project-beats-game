using System;
using PBFramework.Networking.API;

namespace PBGame.Networking.API
{
    public abstract class BaseResponse : IApiResponse {

        protected readonly IHttpRequest request;


        /// <summary>
        /// Whether the response error should be sent to the notification box.
        /// </summary>
        public bool ShouldNotifyError { get; protected set; } = true;

        public bool IsSuccess { get; protected set; }

        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Returns whether the response should store incoming cookies.
        /// </summary>
        protected virtual bool StoresCookies => false;


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
                if (StoresCookies)
                {
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

        public virtual void SetLoginRequired()
        {
            IsSuccess = false;
            ErrorMessage = "You must be logged in first.";
        }
    }
}