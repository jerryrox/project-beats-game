using System.Collections.Generic;
using PBFramework.Threading;

namespace PBGame.Notifications
{
    /// <summary>
    /// A read-only interface of Notification data.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Returns an optional field for identifying notifications.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns the message to be displayed on the notification.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Returns the background image displayed on the notification.
        /// </summary>
        string CoverImage { get; }

        /// <summary>
        /// Returns the displayal scope of the notification.
        /// </summary>
        NotificationScope Scope { get; set; }

        /// <summary>
        /// Returns the list of actions associated with the notification.
        /// </summary>
        IReadOnlyList<NotificationAction> Actions { get; }

        /// <summary>
        /// Returns the type of notification.
        /// </summary>
        NotificationType Type { get; }

        /// <summary>
        /// Returns the background task associated to this notification.
        /// </summary>
        ITask Task { get; }

        /// <summary>
        /// Progress reporter instance for displaying progresses.
        /// </summary>
        TaskListener Listener { get; }
    }

    public static class INotificationExtension
    {
        /// <summary>
        /// Returns whether the notification is potentially safe to be dismissed without affecting
        /// other processes.
        /// </summary>
        public static bool IsDismissible(this INotification context)
        {
            return !HasActions(context) && context.Task == null && context.Listener == null;
        }

        /// <summary>
        /// Returns whether the notification has any associated actions.
        /// </summary>
        public static bool HasActions(this INotification context)
        {
            return context.Actions != null && context.Actions.Count > 0;
        }
    }
}