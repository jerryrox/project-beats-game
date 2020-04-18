using PBGame.UI.Components.Songs;
using PBFramework.UI.Navigations;

namespace PBGame.UI.Navigations.Screens
{
    public interface ISongScreen : INavigationView {
    
        /// <summary>
        /// Returns the search menu component.
        /// </summary>
        SearchMenu SearchMenu { get; }

        /// <summary>
        /// Returns the song list component.
        /// </summary>
        SongList SongList { get; }

        /// <summary>
        /// Returns the song menu component.
        /// </summary>
        SongMenu SongMenu { get; }

        /// <summary>
        /// Returns the background component.
        /// </summary>
        Background Background { get; }
    }
}