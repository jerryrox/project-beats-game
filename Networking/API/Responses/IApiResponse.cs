using System.Threading.Tasks;
using PBFramework.Threading;

namespace PBGame.Networking.API.Responses
{
    public interface IApiResponse {
    
        /// <summary>
        /// Returns whether the response is successful.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Returns the error message of the response, if applicable.
        /// </summary>
        string ErrorMessage { get; }


        /// <summary>
        /// Evaluates the response data to see whether it's a success.
        /// </summary>
        Task Evaluate(IEventProgress progress = null);
    }
}