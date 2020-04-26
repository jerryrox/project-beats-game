using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework;
using PBFramework.Networking;
using PBFramework.Networking.API;

namespace PBGame.Networking.API
{
    public abstract class BaseRequest<T> : IApiRequest
        where T : IApiResponse
    {
        public event Action<T> OnRequestEnd;

        event Action<IApiResponse> IApiRequest.OnRequestEnd
        {
            add => OnRequestEnd += (r) => value.Invoke(r);
            remove => OnRequestEnd -= (r) => value.Invoke(r);
        }


        public IPromise Promise => Requester;

        public T Response { get; private set; }
        IApiResponse IApiRequest.Response => Response;

        public abstract bool RequiresLogin { get; }

        public virtual bool IsNotified => false;

        public virtual string RequestTitle => null;

        public virtual string ResponseTitle => null;

        protected IHttpRequest Requester { get; private set; }

        protected IApi Api { get; private set; }


        public virtual void Prepare(IApi api)
        {
            this.Api = api;

            Requester = CreateRequest();
            Response = CreateResponse(Requester);

            Requester.SetCookies(api.Cookies.GetCookieString());
            Requester.OnFinished += OnHttpResponse;
        }

        public virtual void Request()
        {
            if (RequiresLogin && !Api.IsOnline.Value)
            {
                Response.SetLoginRequired();
                InvokeRequestEnd();
                return;
            }
            Requester.Request();
        }

        public virtual void Dispose()
        {
            if (Requester != null)
            {
                Requester.OnFinished -= OnHttpResponse;
                Requester.Response?.Dispose();
            }
        }

        /// <summary>
        /// Creates a new web request object.
        /// </summary>
        protected abstract IHttpRequest CreateRequest();

        /// <summary>
        /// Creates a new response object.
        /// </summary>
        protected abstract T CreateResponse(IHttpRequest request);

        /// <summary>
        /// Event called from request when finished.
        /// </summary>
        protected virtual void OnHttpResponse()
        {
            Response.Evaluate();
            InvokeRequestEnd();
        }

        /// <summary>
        /// Invokes the request end event to listeners.
        /// </summary>
        protected void InvokeRequestEnd() => OnRequestEnd?.Invoke(Response);
    }
}