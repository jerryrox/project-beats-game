using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.UI.Models.QuickMenu
{
    public class MenuInfo {
    
        /// <summary>
        /// The action to perform on triggering the associated menu button.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// A function which returns whether the menu button should be highlighted.
        /// </summary>
        public Func<bool> HighlightCondition { get; set; }

        /// <summary>
        /// The label text displayed on the button label.
        /// </summary>
        public String Label { get; set; }

        /// <summary>
        /// The sprite name of the menu icon.
        /// </summary>
        public String Icon { get; set; }

        /// <summary>
        /// Returns whether the menu button should be highlighted.
        /// </summary>
        public bool ShouldHighlight => HighlightCondition == null ? false : HighlightCondition.Invoke();
    }
}