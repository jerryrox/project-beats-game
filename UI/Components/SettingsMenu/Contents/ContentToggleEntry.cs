using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Configurations.Settings;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.SettingsMenu.Contents
{
    public class ContentToggleEntry : BaseContentEntry {

        private LabelledToggle toggle;

        private SettingsEntryBool boolEntry;


        protected override float EntryHeight => 48f;


        [InitWithDependency]
        private void Init()
        {
            toggle = CreateChild<LabelledToggle>("toggle");
            {
                toggle.Anchor = AnchorType.Fill;
                toggle.Offset = new Offset(8f);
                toggle.IsClickToTrigger = true;

                toggle.OnFocused += (isFocused) =>
                {
                    if(boolEntry != null && boolEntry.Value != isFocused)
                        boolEntry.Value = isFocused;
                };
            }
        }

        public override void SetEntryData(SettingsEntryBase entryData)
        {
            boolEntry = CastEntryData<SettingsEntryBool>(entryData);

            toggle.LabelText = boolEntry.Name;
            toggle.IsFocused = boolEntry.Value;
        }
    }
}