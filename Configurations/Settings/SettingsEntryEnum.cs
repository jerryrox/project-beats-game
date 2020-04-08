using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Settings
{
    public abstract class SettingsEntryEnum : SettingsEntryBase<string>{

        protected SettingsEntryEnum(string name) : base(name) {}

        /// <summary>
        /// Returns all enum values selectable by the user.
        /// </summary>
        public abstract IEnumerable<string> GetValues();
    }

    public class SettingsEntryEnum<T> : SettingsEntryEnum
        where T : struct
    {
        private IBindable<T> data;

        private Dictionary<string, T> valueMap = new Dictionary<string, T>();


        public override string Value
        {
            get => data.Value.ToString();
            set
            {
                if(valueMap.TryGetValue(value, out T v))
                    data.Value = v;
            }
        }


        public SettingsEntryEnum(string name, IBindable<T> bindable) : base(name)
        {
            this.data = bindable;
            GenerateValueMap();
        }

        public override IEnumerable<string> GetValues() => valueMap.Keys;

        /// <summary>
        /// Generates a dictionary of enum values mapped to their ToString values.
        /// </summary>
        private void GenerateValueMap()
        {
            foreach (var value in (T[])Enum.GetValues(typeof(T)))
                valueMap[value.ToString()] = value;
        }
    }
}