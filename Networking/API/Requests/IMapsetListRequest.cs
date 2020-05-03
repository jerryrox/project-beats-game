using PBGame.Networking.API.Responses;
using PBGame.Networking.Maps;

namespace PBGame.Networking.API.Requests
{
    public interface IMapsetListRequest : IApiRequest<IMapsetListResponse> {
    
        /// <summary>
        /// Name of the cursor value.
        /// </summary>
        string CursorName { get; set; }

        /// <summary>
        /// Value of the cursor under current sort type.
        /// </summary>
        string CursorValue { get; set; }

        /// <summary>
        /// Identifier of the mapset currently on the cursor.
        /// </summary>
        int? CursorId { get; set; }

        /// <summary>
        /// The specific game mode to search for.
        /// </summary>
        int? Mode { get; set; }

        /// <summary>
        /// The specific category to search for.
        /// </summary>
        MapCategoryType Category { get; set; }

        /// <summary>
        /// The specific genre to search for.
        /// </summary>
        MapGenreType Genre { get; set; }

        /// <summary>
        /// The specific language to search for.
        /// </summary>
        MapLanguageType Language { get; set; }

        /// <summary>
        /// The sorting method of mapsets.
        /// </summary>
        MapSortType Sort { get; set; }

        /// <summary>
        /// Whether mapsets are returned in descending order.
        /// </summary>
        bool IsDescending { get; set; }

        /// <summary>
        /// Whether the mapset contains a video.
        /// </summary>
        bool HasVideo { get; set; }

        /// <summary>
        /// Whether the mapset contains a storyboard.
        /// </summary>
        bool HasStoryboard { get; set; }

        /// <summary>
        /// Search keywords.
        /// </summary>
        string SearchTerm { get; set; }
    }
}