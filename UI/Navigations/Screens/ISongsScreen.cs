using PBGame.UI.Components.Songs;
using PBFramework.UI.Navigations;

namespace PBGame.UI.Navigations.Screens
{
    public interface ISongScreen : INavigationView {
    
        /// <summary>
        /// Returns the search menu component.
        /// </summary>
        ISearchMenu SearchMenu { get; }

        /// <summary>
        /// Returns the song list component.
        /// </summary>
        ISongList SongList { get; }

        /// <summary>
        /// Returns the song menu component.
        /// </summary>
        ISongMenu SongMenu { get; }

        /// <summary>
        /// Returns the background component.
        /// </summary>
        IBackground Background { get; }
    }
}