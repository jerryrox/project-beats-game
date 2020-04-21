using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.SettingsMenu.Contents
{
    public class ContentSelectionEntry : BaseContentEntry {

        private ILabel label;
        private DropdownButton dropdown;

        private SettingsEntryEnum enumEntry;
        private DropdownContext dropdownContext;


        protected override float EntryHeight => 78f;


        [InitWithDependency]
        private void Init()
        {
            dropdownContext = new DropdownContext();
            dropdownContext.OnSelection += (data) =>
            {
                if (enumEntry != null)
                    enumEntry.Value = data.Text;
            };

            label = CreateLabel();
            {
                label.Alignment = TextAnchor.UpperLeft;
            }

            dropdown = CreateChild<DropdownButton>("dropdown");
            {
                dropdown.Anchor = Anchors.BottomStretch;
                dropdown.Pivot = Pivots.Bottom;
                dropdown.SetOffsetHorizontal(8f);
                dropdown.Y = 8f;
                dropdown.Height = 30f;
                dropdown.Context = dropdownContext;
            }
        }

        public override void SetEntryData(SettingsEntryBase entryData)
        {
            enumEntry = CastEntryData<SettingsEntryEnum>(entryData);

            label.Text = enumEntry.Name;

            dropdownContext.Datas.Clear();
            foreach (var item in enumEntry.GetValues())
                dropdownContext.Datas.Add(new DropdownData(item));
            dropdownContext.SelectData(dropdownContext.FindData(d => d.Text.Equals(enumEntry.Value, StringComparison.Ordinal)));
        }
    }
}