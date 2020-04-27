using PBFramework;
using PBFramework.Graphics;

namespace PBGame.Notifications
{
    public interface INotification {

        /// <summary>
        /// An optional identifier provided by the emitter.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// The message to be displayed on the notification.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// A link to the image covered on the background of the notification.
        /// </summary>
        string CoverImage { get; set; }

        /// <summary>
        /// Type of notification displaying scope.
        /// </summary>
        NotificationScope Scope { get; set; }

        /// <summary>
        /// The default action to perform on pressing the notification.
        /// </summary>
        NotificationAction DefaultAction { get; set; }

        /// <summary>
        /// Type of notification message.
        /// </summary>
        NotificationType Type { get; set; }

        /// <summary>
        /// Promise instance bound to the notification.
        /// If specified,
        /// - Notification's progress will be referred from this object.
        /// - Notification will be removed automatically if the promise has finished.
        /// </summary>
        IPromise Promise { get; set; }


        /// <summary>
        /// Adds an optional action that can be performed from the notification.
        /// </summary>
        void AddAction(NotificationAction action);
    }
}