using System.Collections.Generic;

namespace PBGame.Configurations.Settings
{
    /// <summary>
    /// An object which provides the definitions for building settings layout.
    /// </summary>
    public interface ISettingsData {
    
        /// <summary>
        /// Returns the number of tabs in the data.
        /// </summary>
        int TabCount { get; }


        /// <summary>
        /// Returns all tabs in the settings.
        /// </summary>
        IEnumerable<SettingsTab> GetTabs();

        /// <summary>
        /// Adds the specified tab data.
        /// Returns the specified tab instance.
        /// </summary>
        SettingsTab AddTabData(SettingsTab tabData);        
    }
}