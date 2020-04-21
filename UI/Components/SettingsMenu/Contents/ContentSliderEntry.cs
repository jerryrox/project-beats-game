using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.SettingsMenu.Contents
{
    public abstract class ContentSliderEntry<T> : BaseContentEntry
        where T : struct
    {
        protected BasicSlider slider;
        private ILabel label;
        private ILabel valueLabel;

        protected SettingsEntryRange<T> rangeEntry;


        protected override float EntryHeight => 64f;

        /// <summary>
        /// The string format mode to use to display the slider value.
        /// </summary>
        protected virtual string ValueFormat => "";


        [InitWithDependency]
        private void Init()
        {
            label = CreateLabel();
            {
                label.Alignment = TextAnchor.UpperLeft;
            }
            valueLabel = CreateLabel();
            {
                valueLabel.Alignment = TextAnchor.UpperRight;
            }

            slider = CreateChild<BasicSlider>("slider", 1);
            {
                slider.Anchor = Anchors.BottomStretch;
                slider.Pivot = Pivots.Bottom;
                slider.SetOffsetHorizontal(8f);
                slider.Y = 8f;
                slider.Height = 16f;

                slider.OnChange += (value) =>
                {
                    valueLabel.Text = value.ToString(ValueFormat);
                };
            }
        }

        public override void SetEntryData(SettingsEntryBase entryData)
        {
            rangeEntry = CastEntryData<SettingsEntryRange<T>>(entryData);

            label.Text = entryData.Name;
            slider.IsWholeNumber = rangeEntry is SettingsEntryInt;
        }
    }
}