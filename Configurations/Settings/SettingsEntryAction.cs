using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Configurations.Settings
{
    /// <summary>
    /// Settings entry which represents an action that can modify certain configurations on invocation.
    /// </summary>
    public class SettingsEntryAction : SettingsEntryBase {

        private Action action;


        public SettingsEntryAction(string name, Action action) : base(name)
        {
            this.action = action;
        }

        /// <summary>
        /// Invokes the action associated.
        /// </summary>
        public void Invoke() => action?.Invoke();
    }
}