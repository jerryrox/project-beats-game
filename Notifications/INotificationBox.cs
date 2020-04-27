using System;

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
        /// Adds a notification as a quick message 
        /// </summary>
        void Add(INotification notification);

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