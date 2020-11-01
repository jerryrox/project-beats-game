using System.Collections.Generic;
using PBFramework.Threading;

namespace PBGame.Notifications
{
    public class Notification : INotification {

        public string Id { get; set; }

        public string Message { get; set; }

        public string CoverImage { get; set; }

        public NotificationScope Scope { get; set; } = NotificationScope.Temporary;

        public List<NotificationAction> Actions { get; set; }
        IReadOnlyList<NotificationAction> INotification.Actions => Actions?.AsReadOnly();

        public NotificationType Type { get; set; } = NotificationType.Passive;

        public ITask Task { get; set; }

        public TaskListener Listener { get; set; }
    }
}