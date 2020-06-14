using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.UI.Components.Common.Dropdown
{
    /// <summary>
    /// Data structure representing a single entry in dropdown menu.
    /// </summary>
    public class DropdownData {

        /// <summary>
        /// The text value displayed on the menu.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// An extra data associated with this entry, if needed.
        /// </summary>
        public object ExtraData { get; set; }


        public DropdownData(string text) : this(text, null) { }

        public DropdownData(string text, object extraData)
        {
            this.Text = text;
            this.ExtraData = extraData;
        }
    }
}