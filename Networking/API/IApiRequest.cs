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
        /// Returns whether this request requires login.
        /// </summary>
        bool RequiresLogin { get; }


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