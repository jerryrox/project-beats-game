using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Settings
{
    public class SettingsEntryInt : SettingsEntryRange<int>
    {
        public SettingsEntryInt(string name, IBindableNumber<int> data) : base(name, data) { }
    }
}