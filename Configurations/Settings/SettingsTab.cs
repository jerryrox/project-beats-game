using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Configurations.Settings
{
    /// <summary>
    /// An object holding metadata of the tab itself and all the entries included.
    /// </summary>
    public class SettingsTab {

        /// <summary>
        /// The displayed name of the tab.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The spritename of the tab icon.
        /// </summary>
        public string IconName { get; private set; }


        public SettingsTab(string name, string iconName)
        {
            this.Name = name;
            this.IconName = iconName;
        }


    }
}