using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations.Settings;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.SettingsMenu.Contents
{
    public class ContentIntEntry : ContentSliderEntry<int> {

        [InitWithDependency]
        private void Init()
        {
            slider.OnChange += (value) =>
            {
                int iValue = (int)value;
                if(iValue != rangeEntry.Value && rangeEntry != null)
                    rangeEntry.Value = iValue;
            };
        }

        public override void SetEntryData(SettingsEntryBase entryData)
        {
            base.SetEntryData(entryData);
            slider.IsWholeNumber = true;
            slider.MinValue = rangeEntry.MinValue;
            slider.MaxValue = rangeEntry.MaxValue;
            slider.Value = rangeEntry.Value;
        }
    }
}