using System.Collections.Generic;
using PBFramework.Threading;

namespace PBGame.Notifications
{
    public class Notification {

        /// <summary>
        /// An optional field for identifying notifications.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Message to be displayed on the notification.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Background image displayed on the notification.
        /// </summary>
        public string CoverImage { get; set; }

        /// <summary>
        /// Displayal scope of the notification.
        /// </summary>
        public NotificationScope Scope { get; set; } = NotificationScope.Stored;

        /// <summary>
        /// List of actions associated with the notification.
        /// </summary>
        public List<NotificationAction> Actions { get; set; }

        /// <summary>
        /// Returns whether there is at least one action on this notification.
        /// </summary>
        public bool HasActions => Actions != null && Actions.Count > 0;

        /// <summary>
        /// Type of notification.
        /// </summary>
        public NotificationType Type { get; set; } = NotificationType.Passive;

        /// <summary>
        /// A background task associated to this notification.
        /// </summary>
        public ITask Task { get; set; }
    }
}