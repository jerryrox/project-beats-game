using System.Collections.Generic;
using PBGame.Rulesets.Maps;

namespace PBGame.Maps
{
    public interface IMusicPlaylist {

        /// <summary>
        /// Clears the mapsets in the playlist.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears and refills the playlist using the specified list.
        /// </summary>
        void Refill(List<IMapset> mapsets);

        /// <summary>
        /// Changes focusing cursor on the specified mapset.
        /// </summary>
        void Focus(IMapset mapset);

        /// <summary>
        /// Returns the next mapset in the list.
        /// Moves cursor to the next mapset.
        /// </summary>
        IMapset Next();

        /// <summary>
        /// Returns the next mapset in the list.
        /// </summary>
        IMapset PeekNext();

        /// <summary>
        /// Returns the previous mapset in the list.
        /// Moves cursor to the previous mapset.
        /// </summary>
        IMapset Previous();

        /// <summary>
        /// Returns the previous mapset in the list.
        /// </summary>
        IMapset PeekPrevious();
    }
}