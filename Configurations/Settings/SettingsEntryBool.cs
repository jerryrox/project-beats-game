using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Settings
{
    public class SettingsEntryBool : SettingsEntryBase<bool> {

        public SettingsEntryBool(string name, IBindable<bool> bindable) : base(name, bindable) {}
    }
}