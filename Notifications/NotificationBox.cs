using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Notifications
{
    public class NotificationBox : INotificationBox {

        public event Action<INotification> OnNewNotification;

        public event Action<INotification> OnRemoveNotification;

        private List<INotification> notifications = new List<INotification>();


        public void Add(INotification notification)
        {
            if(notification.Scope != NotificationScope.Temporary)
                notifications.Add(notification);
            OnNewNotification?.Invoke(notification);
        }

        public void Remove(INotification notification)
        {
            if(notifications.Remove(notification))
                OnRemoveNotification?.Invoke(notification);
        }

        public void RemoveById(string id, bool multiple)
        {
            if(string.IsNullOrEmpty(id))
                return;

            for (int i = notifications.Count - 1; i >= 0; i--)
            {
                var notif = notifications[i];
                if (notif.Id == id)
                {
                    notifications.RemoveAt(i);
                    OnRemoveNotification?.Invoke(notif);
                    if(!multiple)
                        return;
                }
            }
        }
    }
}