using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Settings
{
    public abstract class SettingsEntryRange<T> : SettingsEntryBase<T>
        where T : struct
    {
        protected BindableNumber<T> data;


        public T MinValue => data.MinValue;

        public T MaxValue => data.MaxValue;


        protected SettingsEntryRange(string name, BindableNumber<T> data) : base(name)
        {
            this.data = data;
        }
    }
}