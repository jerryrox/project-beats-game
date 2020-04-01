using System;
using PBGame.Maps;
using PBFramework.UI;

namespace PBGame.UI.Components.Songs
{
    public interface ISortButton : IHighlightTrigger {

        /// <summary>
        /// The sort type represented by this button.
        /// </summary>
        MapsetSorts SortType { get; set; }
    }
}