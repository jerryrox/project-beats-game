namespace PBGame.Networking.API
{
    public interface IApiResponse {
    
        /// <summary>
        /// Returns whether the request was a success.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Returns the error message of the request, if exists.
        /// </summary>
        string ErrorMessage { get; }


        /// <summary>
        /// Evaluates the response data and determines whether it's a success.
        /// </summary>
        void Evaluate();

        /// <summary>
        /// Applies any state changes to the api, if applicable.
        /// </summary>
        void ApplyResponse(IApi api);

        /// <summary>
        /// Sets the response result to failed, noting that login is required.
        /// </summary>
        void SetLoginRequired();
    }
}