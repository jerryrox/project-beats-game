using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Settings
{
    public class SettingsEntryString : SettingsEntryBase<string> {

        public SettingsEntryString(string name, IBindable<string> bindable) : base(name, bindable) { }
    }
}