using System;
using PBGame.Maps;
using PBFramework.UI;

namespace PBGame.UI.Components.Songs
{
    public interface ISortButton : ITrigger {

        /// <summary>
        /// The sort type represented by this button.
        /// </summary>
        MapsetSorts SortType { get; set; }

        /// <summary>
        /// The text on the button label.
        /// </summary>
        string LabelText { get; set; }


        /// <summary>
        /// Sets the toggle state on the button.
        /// </summary>
        void SetToggle(bool isToggled);
    }
}