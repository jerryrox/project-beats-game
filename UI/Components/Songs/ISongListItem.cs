using PBGame.Rulesets.Maps;
using PBFramework.UI;

namespace PBGame.UI.Components.Songs
{
    public interface ISongListItem : IListItem {

        /// <summary>
        /// Returns the mapset currently being represented by this item.
        /// </summary>
        IMapset Mapset { get; }


        /// <summary>
        /// Sets the mapset which the item should represent.
        /// </summary>
        void SetMapset(IMapset mapset);
    }
}