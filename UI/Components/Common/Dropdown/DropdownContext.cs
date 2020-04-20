using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.UI.Components.Common.Dropdown
{
    /// <summary>
    /// A data structure for containing dropdown data and mediating events betwen menu and caller.
    /// </summary>
    public class DropdownContext {

        /// <summary>
        /// Event called on dropdown data selection.
        /// </summary>
        public event Action<DropdownData> OnSelection;


        /// <summary>
        /// Returns the currently selected data.
        /// </summary>
        public DropdownData Selection { get; private set; }

        /// <summary>
        /// List of datas registered in the context.
        /// </summary>
        public List<DropdownData> Datas { get; private set; } = new List<DropdownData>();


        /// <summary>
        /// Invokes selection of specified dropdown data.
        /// </summary>
        public void SelectData(DropdownData data)
        {
            Selection = data;
            OnSelection?.Invoke(data);
        }
    }
}