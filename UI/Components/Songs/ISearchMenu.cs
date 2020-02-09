using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.UI.Components.Songs
{
    public interface ISearchMenu : IGraphicObject {
    
        /// <summary>
        /// Returns the search bar.
        /// </summary>
        ISearchBar SearchBar { get; }

        /// <summary>
        /// Returns the mapset sorter object.
        /// </summary>
        ISorter Sorter { get; }
    }
}