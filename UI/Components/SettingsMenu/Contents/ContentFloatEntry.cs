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
            if(this.rangeEntry != null)
                this.rangeEntry.OnDataValueChange -= OnEntryValueChange;

            base.SetEntryData(entryData);
            rangeEntry.OnDataValueChange += OnEntryValueChange;
            
            slider.MinValue = rangeEntry.MinValue;
            slider.MaxValue = rangeEntry.MaxValue;
            slider.Value = rangeEntry.Value;
        }

        /// <summary>
        /// Event called from entry data when its value has changed.
        /// </summary>
        private void OnEntryValueChange(float value) => slider.Value = value;
    }
}