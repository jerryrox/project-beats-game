using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.UI.Components.Common.Dropdown
{
    /// <summary>
    /// A data structure for containing dropdown data and mediating events betwen menu and caller.
    /// </summary>
    public class DropdownContext {

        private Action<DropdownData> callback;


        /// <summary>
        /// Returns the currently selected data.
        /// </summary>
        public DropdownData Selection { get; private set; }

        /// <summary>
        /// List of datas registered in the context.
        /// </summary>
        public List<DropdownData> Datas { get; private set; } = new List<DropdownData>();


        public DropdownContext(Action<DropdownData> callback)
        {
            this.callback = callback;
        }

        /// <summary>
        /// Invokes selection of specified dropdown data.
        /// </summary>
        public void SelectData(DropdownData data)
        {
            Selection = data;
            callback?.Invoke(data);
        }
    }
}