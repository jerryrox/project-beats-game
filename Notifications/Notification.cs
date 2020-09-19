using System.Collections.Generic;
using PBFramework.Threading;

namespace PBGame.Notifications
{
    public class Notification : INotification {

        private List<NotificationAction> actions;


        public string Id { get; set; }

        public string Message { get; set; }

        public string CoverImage { get; set; }

        public NotificationScope Scope { get; set; } = NotificationScope.Stored;

        public NotificationAction DefaultAction { get; set; }

        public bool HasActions => actions != null && actions.Count > 0;

        public NotificationType Type { get; set; } = NotificationType.Passive;

        public TaskListener Listener { get; set; }


        public void AddAction(NotificationAction action)
        {
            if(actions == null)
                actions = new List<NotificationAction>();
            actions.Add(action);
        }

        public IEnumerable<NotificationAction> GetActions()
        {
            if(actions == null)
                yield break;
            foreach(var action in actions)
                yield return action;
        }
    }
}