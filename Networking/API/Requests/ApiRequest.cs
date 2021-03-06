using System;
using PBGame.Networking.API.Responses;
using PBGame.Notifications;
using PBFramework.Data.Bindables;
using PBFramework.Threading;
using PBFramework.Networking;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Requests
{
    public abstract class ApiRequest<T> : IApiRequest<T>
        where T : IApiResponse
    {
        public event Action OnDisposing;

        protected IApi api;
        protected IApiProvider provider;

        private Bindable<T> response = new Bindable<T>();


        public bool DidRequest { get; private set; }

        public IHttpRequest InnerRequest { get; private set; }

        public IReadOnlyBindable<T> Response => response;

        IReadOnlyBindable IApiRequest.RawResponse => response;


        protected ApiRequest(IApi api, IApiProvider provider)
        {
            if(api == null)
                throw new ArgumentNullException(nameof(api));
            if(provider == null)
                throw new ArgumentNullException(nameof(provider));

            this.api = api;
            this.provider = provider;

            InnerRequest = CreateRequest();
        }

        public void Request(TaskListener<IWebRequest> listener = null)
        {
            if (DidRequest)
                throw new Exception("This request has already been made!");
            if(InnerRequest == null)
                throw new Exception("This request has already been disposed!");

            DidRequest = true;

            InnerRequest.OnFinished += OnHttpResponse;

            OnPreRequest();
            InnerRequest.Request(listener: listener);
        }

        public virtual Notification CreateNotification() => null;

        public void Dispose()
        {
            if (InnerRequest != null)
            {
                OnDisposing?.Invoke();

                InnerRequest.OnFinished -= OnHttpResponse;
                InnerRequest.Response?.Dispose();
                InnerRequest = null;
            }
        }

        /// <summary>
        /// Creates a new requester object.
        /// </summary>
        protected abstract IHttpRequest CreateRequest();

        /// <summary>
        /// Creates a new response object using current request.
        /// </summary>
        protected abstract T CreateResponse(IHttpRequest request);

        /// <summary>
        /// Event called before requesting.
        /// </summary>
        protected virtual void OnPreRequest() { }

        /// <summary>
        /// Event called on inner request response.
        /// </summary>
        private void OnHttpResponse(IWebRequest request)
        {
            T response = CreateResponse(InnerRequest);
            response.OnEvaluated += () =>
            {
                this.response.Value = response;
                Dispose();
            };

            response.Evaluate();
        }
    }
}