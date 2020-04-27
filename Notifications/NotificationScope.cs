namespace PBGame.Notifications
{
    /// <summary>
    /// Types of notification display scope.
    /// </summary>
    public enum NotificationScope {

        /// <summary>
        /// Notification is displayed for both quick message and notification panels.
        /// </summary>
        All,

        /// <summary>
        /// Notification is displayed for quick message only.
        /// </summary>
        QuickOnly,

        /// <summary>
        /// Notification is displayed for notification panel only.
        /// </summary>
        NotificationOnly,
    }
}