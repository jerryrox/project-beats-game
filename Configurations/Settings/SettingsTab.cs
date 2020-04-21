using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Configurations.Settings
{
    /// <summary>
    /// An object holding metadata of the tab itself and all the entries included.
    /// </summary>
    public class SettingsTab {

        private List<SettingsEntryBase> entries = new List<SettingsEntryBase>();


        /// <summary>
        /// The displayed name of the tab.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The spritename of the tab icon.
        /// </summary>
        public string IconName { get; private set; }

        /// <summary>
        /// Returns the number of entries in the tab.
        /// </summary>
        public int EntryCount => entries.Count;

        /// <summary>
        /// Returns the entry data at specified index.
        /// </summary>
        public SettingsEntryBase this[int index] => entries[index];


        public SettingsTab(string name, string iconName)
        {
            this.Name = name;
            this.IconName = iconName;
        }

        /// <summary>
        /// Adds the specified entry to the tab.
        /// </summary>
        public void AddEntry(SettingsEntryBase entry) => entries.Add(entry);

        /// <summary>
        /// Returns all entries in the tab.
        /// </summary>
        public IEnumerable<SettingsEntryBase> GetEntries() => entries;
    }
}