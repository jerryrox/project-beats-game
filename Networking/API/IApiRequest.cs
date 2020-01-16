using System;
using PBFramework;

namespace PBGame.Networking.API
{
    /// <summary>
    /// A general view of an API request.
    /// </summary>
    public interface IApiRequest : IDisposable
    {
        /// <summary>
        /// Event called when the request has finished.
        /// </summary>
        event Action<IApiResponse> OnRequestEnd;


        /// <summary>
        /// Returns the request represented as a promise.
        /// Will not be available before being requested by the API provider.
        /// </summary>
        IPromise Promise { get; }

        /// <summary>
        /// Returns the response container of the request.
        /// Will not be available before being requested by the API provider.
        /// </summary>
        IApiResponse Response { get; }

        /// <summary>
        /// Returns whether the requesting progress should be displayed as a notification.
        /// </summary>
        bool IsNotified { get; }

        /// <summary>
        /// If notifying request, the displayed name of the notification on the notification entry DURING REQUEST.
        /// </summary>
        string RequestTitle { get; }

        /// <summary>
        /// If notifying request, the displayed name of the notification on the notification entry ON FINISHED.
        /// </summary>
        string ResponseTitle { get; }


        /// <summary>
        /// Prepares request before executing it.
        /// </summary>
        void Prepare(IApi api);

        /// <summary>
        /// Sends the request.
        /// </summary>
        void Request();
    }

    public interface IApiRequest<T> : IApiRequest
        where T : IApiResponse
    {
        /// <summary>
        /// Event called when the request has finished.
        /// </summary>
        new event Action<T> OnRequestEnd;
    }
}