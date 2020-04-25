using System;
using System.Linq;
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

        /// <summary>
        /// Imports data from specified enum.
        /// </summary>
        public void ImportFromEnum<T>(T initialValue = default)
            where T : Enum
        {
            Clear();

            foreach (var type in (T[])Enum.GetValues(typeof(T)))
            {
                DropdownData data = new DropdownData(type.ToString(), type);
                Datas.Add(data);

                if(type.Equals(initialValue))
                    SelectData(data);
            }
        }

        /// <summary>
        /// Clears all context data except event emitters.
        /// </summary>
        public void Clear()
        {
            SelectData(null);
            Datas.Clear();
        }

        /// <summary>
        /// Returns the data matching the specified predicate.
        /// </summary>
        public DropdownData FindData(Predicate<DropdownData> predicate) => Datas.Find(predicate);
    }
}