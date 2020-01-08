using PBGame.Rulesets.Maps;

namespace PBGame.Maps
{
    public interface IMusicPlaylist {

        /// <summary>
        /// Refills the playlist.
        /// </summary>
        void Refill();

        /// <summary>
        /// Changes focusing cursor on the specified mapset.
        /// </summary>
        void Focus(IMapset mapset);

        /// <summary>
        /// Returns the next mapset in the list.
        /// Moves cursor to the next mapset.
        /// </summary>
        IMapset GetNext();

        /// <summary>
        /// Returns the current mapset under cursor.
        /// </summary>
        IMapset GetCurrent();

        /// <summary>
        /// Returns the previous mapset in the list.
        /// Moves cursor to the previous mapset.
        /// </summary>
        IMapset GetPrevious();
    }
}