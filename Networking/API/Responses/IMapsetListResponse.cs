using PBGame.Networking.Maps;

namespace PBGame.Networking.API.Responses
{
    public interface IMapsetListResponse : IApiResponse
    {
        /// <summary>
        /// Returns the array of mapsets retrieved from the server.
        /// </summary>
        OnlineMapset[] Mapsets { get; }

        /// <summary>
        /// Returns the name of the cursor for cursor value.
        /// </summary>
        string CursorName { get; }

        /// <summary>
        /// Returns the cursor value for querying next page.
        /// </summary>
        string CursorValue { get; }

        /// <summary>
        /// Returns the cursor id for querying next page.
        /// </summary>
        int CursorId { get; }

        /// <summary>
        /// Returns the total results of the search.
        /// </summary>
        int Total { get; }
    }
}