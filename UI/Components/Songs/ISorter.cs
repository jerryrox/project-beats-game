using PBGame.Maps;
using PBFramework.Graphics;

namespace PBGame.UI.Components.Songs
{
    public interface ISorter : IGraphicObject {

        /// <summary>
        /// Sets the sorting method of the mapsets.
        /// </summary>
        void SetSort(MapsetSorts sorts);
    }
}