using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Settings
{
    public abstract class SettingsEntryRange<T> : SettingsEntryBase<T>
        where T : struct
    {
        protected IBindableNumber<T> data;


        public T MinValue => data.MinValue;

        public T MaxValue => data.MaxValue;


        protected SettingsEntryRange(string name, IBindableNumber<T> data) : base(name, data)
        {
            this.data = data;
        }
    }
}