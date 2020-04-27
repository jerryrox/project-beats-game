namespace PBGame.Notifications
{
    /// <summary>
    /// Types of notification display scope.
    /// </summary>
    public enum NotificationScope {

        /// <summary>
        /// Notification is only broadcast through event and not stored.
        /// </summary>
        Temporary,

        /// <summary>
        /// Notification is broadcast through event and stored in notifications list.
        /// </summary>
        Stored,
    }
}