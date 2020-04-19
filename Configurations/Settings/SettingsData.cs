using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Configurations.Settings
{
    public class SettingsData : ISettingsData {

        private List<SettingsTab> tabs = new List<SettingsTab>();


        public int TabCount => tabs.Count;

        public SettingsTab this[int index] => tabs[index];


        public IEnumerable<SettingsTab> GetTabs() => tabs;

        public SettingsTab AddTabData(SettingsTab tabData)
        {
            tabs.Add(tabData);
            return tabData;
        }
    }
}