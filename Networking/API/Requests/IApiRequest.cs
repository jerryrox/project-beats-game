using System;
using PBGame.Networking.API.Responses;
using PBGame.Notifications;
using PBFramework.Data.Bindables;
using PBFramework.Threading;
using PBFramework.Networking;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Requests
{
    public interface IApiRequest : IDisposable
    {
        /// <summary>
        /// Event called before the request is disposed.
        /// </summary>
        event Action OnDisposing;


        /// <summary>
        /// Returns whether this request has already been made.
        /// </summary>
        bool DidRequest { get; }

        /// <summary>
        /// Returns the object which handles the actual web request.
        /// </summary>
        IHttpRequest InnerRequest { get; }

        /// <summary>
        /// Returns the bindable raw response of this request.
        /// </summary>
        IReadOnlyBindable RawResponse { get; }


        /// <summary>
        /// Starts requesting the PB Api server for a response.
        /// </summary>
        void Request(TaskListener<IWebRequest> listener = null);

        /// <summary>
        /// Creates a new notification that represents this request.
        /// </summary>
        INotification CreateNotification();
    }

    public interface IApiRequest<T> : IApiRequest
        where T : IApiResponse
    {
        /// <summary>
        /// Returns the bindable response of this request.
        /// </summary>
        IReadOnlyBindable<T> Response { get; }
    }
}