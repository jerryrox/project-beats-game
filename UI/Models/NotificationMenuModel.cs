using System;
using System.Collections.Generic;
using PBGame.Notifications;
using PBFramework.Dependencies;

namespace PBGame.UI.Models
{
    public class NotificationMenuModel : BaseModel
    {
        /// <summary>
        /// Event called when a new notification is added.
        /// </summary>
        public event Action<INotification> OnNewNotification;

        /// <summary>
        /// Event called when an existing notification was removed.
        /// </summary>
        public event Action<INotification> OnRemoveNotification;


        /// <summary>
        /// Returns the list of notifications in the box.
        /// </summary>
        public IReadOnlyList<INotification> Notifications => NotificationBox.Notifications;

        [ReceivesDependency]
        private INotificationBox NotificationBox { get; set; }


        [InitWithDependency]
        private void Init()
        {
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            NotificationBox.OnNewNotification += OnNotificationAdded;
            NotificationBox.OnRemoveNotification += OnNotificationRemoved;
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            NotificationBox.OnNewNotification -= OnNotificationAdded;
            NotificationBox.OnRemoveNotification -= OnNotificationRemoved;
        }

        /// <summary>
        /// Event called from notification box when a new notification is added.
        /// </summary>
        private void OnNotificationAdded(INotification notification)
        {
            OnNewNotification?.Invoke(notification);
        }

        /// <summary>
        /// Event called from notification box when a notification is removed.
        /// </summary>
        private void OnNotificationRemoved(INotification notification)
        {
            OnRemoveNotification?.Invoke(notification);
        }
    }
}