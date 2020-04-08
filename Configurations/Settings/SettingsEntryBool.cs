using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Settings
{
    public class SettingsEntryBool : SettingsEntryBase<bool> {

        public SettingsEntryBool(string name, BindableBool bindable) : base(name, bindable) {}
    }
}