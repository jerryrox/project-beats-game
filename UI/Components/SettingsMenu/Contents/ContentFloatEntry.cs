using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations.Settings;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.SettingsMenu.Contents
{
    public class ContentFloatEntry : ContentSliderEntry<float>
    {
        [InitWithDependency]
        private void Init()
        {
            slider.OnChange += (value) =>
            {
                if (value != rangeEntry.Value && rangeEntry != null)
                    rangeEntry.Value = value;
            };
        }

        public override void SetEntryData(SettingsEntryBase entryData)
        {
            base.SetEntryData(entryData);
            slider.MinValue = rangeEntry.MinValue;
            slider.MaxValue = rangeEntry.MaxValue;
            slider.Value = rangeEntry.Value;
        }
    }
}