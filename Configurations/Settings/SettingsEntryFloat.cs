using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Settings
{
    public class SettingsEntryFloat : SettingsEntryRange<float> {

        public SettingsEntryFloat(string name, IBindableNumber<float> data) : base(name, data) {}
    }
}