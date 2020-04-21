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
    public class ContentInputEntry : BaseContentEntry {

        private BasicInput input;
        private ILabel label;

        private SettingsEntryString stringEntry;


        protected override float EntryHeight => 74f;


        [InitWithDependency]
        private void Init()
        {
            label = CreateLabel();
            {
                label.Alignment = TextAnchor.UpperLeft;
            }

            input = CreateChild<BasicInput>("input", 1);
            {
                input.Anchor = Anchors.BottomStretch;
                input.Pivot = Pivots.Bottom;
                input.SetOffsetHorizontal(8f);
                input.Y = 8f;
                input.Height = 30f;

                input.UseDefaultHoverAni();
                input.UseDefaultFocusAni();

                input.OnSubmitted += (value) =>
                {
                    if(stringEntry != null && stringEntry.Value != value)
                        stringEntry.Value = value;
                };
            }
        }

        public override void SetEntryData(SettingsEntryBase entryData)
        {
            stringEntry = CastEntryData<SettingsEntryString>(entryData);

            label.Text = stringEntry.Name;
            input.Text = stringEntry.Value;
        }
    }
}