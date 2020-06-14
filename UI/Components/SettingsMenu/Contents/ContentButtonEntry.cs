using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.SettingsMenu.Contents
{
    public class ContentButtonEntry : BaseContentEntry {

        private BoxButton button;

        private SettingsEntryAction actionEntry;


        protected override float EntryHeight => 52f;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            button = CreateChild<BoxButton>("button");
            {
                button.Anchor = AnchorType.Fill;
                button.Offset = new Offset(16f, 8f, 16f, 8f);
                button.Color = colorPreset.Passive;

                button.OnTriggered += () => actionEntry?.Invoke();
            }
        }

        public override void SetEntryData(SettingsEntryBase entryData)
        {
            actionEntry = CastEntryData<SettingsEntryAction>(entryData);

            button.LabelText = entryData.Name;
        }
    }
}