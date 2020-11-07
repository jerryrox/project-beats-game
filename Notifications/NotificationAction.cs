using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Notifications
{
    /// <summary>
    /// Contains information related to an action associated with a notification.
    /// </summary>
    public class NotificationAction {

        /// <summary>
        /// The action to perform on invocation.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// The displayed name of the action.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether this action should dismiss the notification.
        /// </summary>
        public bool ShouldDismiss { get; set; } = false;
    }
}