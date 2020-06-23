using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Settings
{
    /// <summary>
    /// An object representing the metadata of a configuration entry as displayed on UI.
    /// </summary>
    public abstract class SettingsEntryBase {

        /// <summary>
        /// Displayed name of the settings entry.
        /// </summary>
        public string Name { get; private set; }


        protected SettingsEntryBase(string name)
        {
            this.Name = name;
        }
    }

    public abstract class SettingsEntryBase<T> : SettingsEntryBase
    {
        /// <summary>
        /// Event called when the bindable data's value has changed.
        /// </summary>
        public event Action<T> OnDataValueChange;

        /// <summary>
        /// The bindable data to be serverd automatically at this class's level.
        /// May be null if data binding will be handled manually.
        /// </summary>
        private IBindable<T> data;


        /// <summary>
        /// The value of the entry.
        /// </summary>
        public virtual T Value
        {
            get => data == null ? default(T) : data.Value;
            set
            {
                if(data != null)
                    data.Value = value;
            }
        }


        protected SettingsEntryBase(string name) : base(name) { }

        protected SettingsEntryBase(string name, IBindable<T> bindable) : base(name)
        {
            this.data = bindable;
            if (bindable != null)
            {
                bindable.OnNewValue += (value) =>
                {
                    OnDataValueChange?.Invoke(value);
                };
            }
        }
    }
}