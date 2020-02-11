using PBGame.Maps;

namespace PBGame.UI.Components.MenuBar
{
    public interface IMusicButton : IMenuButton {
    
        /// <summary>
        /// Returns the playlist currently being managed.
        /// </summary>
        IMusicPlaylist MusicPlaylist { get; }


        /// <summary>
        /// Sets current music to next in the list.
        /// </summary>
        void SetNextMusic();

        /// <summary>
        /// Sets current music to prev in the list.
        /// </summary>
        void SetPrevMusic();

        /// <summary>
        /// Sets current music to random mapset in the list.
        /// </summary>
        void SetRandomMusic();
    }
}