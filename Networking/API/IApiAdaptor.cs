using PBGame.Networking.Maps;

namespace PBGame.Networking.API
{
    /// <summary>
    /// An interface for integrating similar concepts with inconsistent field data/names/values.
    /// </summary>
    public interface IApiAdaptor {

        /// <summary>
        /// Returns the server field name for the specified map sorting type.
        /// </summary>
        string GetMapSortName(MapSortType sortType, bool isDescending);
    }
}