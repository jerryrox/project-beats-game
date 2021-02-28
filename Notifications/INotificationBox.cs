using System;
using System.Collections.Generic;

namespace PBGame.Notifications
{
    /// <summary>
    /// Interface of a notification container.
    /// </summary>
    public interface INotificationBox {

        /// <summary>
        /// Event called on new notification.
        /// </summary>
        event Action<INotification> OnNewNotification;

        /// <summary>
        /// Event called on notification removal.
        /// </summary>
        event Action<INotification> OnRemoveNotification;

        /// <summary>
        /// If non-null, the minimum level of notification that should implicitly be treated as a stored-scope notification.
        /// </summary>
        NotificationType? ForceStoreLevel { get; set; }

        /// <summary>
        /// Returns the list of notifications currently stored in the box.
        /// </summary>
        IReadOnlyList<INotification> Notifications { get; }


        /// <summary>
        /// Adds a notification as a quick message 
        /// </summary>
        void Add(Notification notification);

        /// <summary>
        /// Removes the specified notification from the list.
        /// </summary>
        void Remove(INotification notification);

        /// <summary>
        /// Removes the notification of specified id.
        /// </summary>
        void RemoveById(string id, bool multiple);
    }
}