using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;

namespace PBGame.Notifications
{
    public class NotificationBox : INotificationBox {

        public event Action<INotification> OnNewNotification;

        public event Action<INotification> OnRemoveNotification;

        private List<INotification> notifications = new List<INotification>();


        public void Add(INotification notification)
        {
            UnityThread.DispatchUnattended(() =>
            {
                if (notification.Scope != NotificationScope.Temporary)
                    notifications.Add(notification);
                OnNewNotification?.Invoke(notification);
                return null;
            });
        }

        public void Remove(INotification notification)
        {
            UnityThread.DispatchUnattended(() =>
            {
                if (notifications.Remove(notification))
                    OnRemoveNotification?.Invoke(notification);
                return null;
            });
        }

        public void RemoveById(string id, bool multiple)
        {
            if(string.IsNullOrEmpty(id))
                return;

            UnityThread.DispatchUnattended(() =>
            {
                for (int i = notifications.Count - 1; i >= 0; i--)
                {
                    var notif = notifications[i];
                    if (notif.Id == id)
                    {
                        notifications.RemoveAt(i);
                        OnRemoveNotification?.Invoke(notif);
                        if (!multiple)
                            return null;
                    }
                }
                return null;
            });
        }
    }
}