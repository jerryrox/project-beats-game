using PBFramework.Debugging;
using PBFramework.Threading;

namespace PBGame.Notifications
{
    /// <summary>
    /// Log service implementation which pipes incoming log messages to the notification box.
    /// </summary>
    public class LogToNotificationService : ILogService
    {
        /// <summary>
        /// The notification box instance to pipe logs to.
        /// </summary>
        public NotificationBox NotificationBox { get; set; }

        /// <summary>
        /// The minimum level of log type which allows log messages to be piped to the notification box.
        /// </summary>
        public LogType PipeLogLevel { get; set; } = LogType.Verbose;


        public void LogVerbose(object message) => Pipe(LogType.Verbose, message);

        public void LogInfo(object message) => Pipe(LogType.Info, message);

        public void LogWarning(object message) => Pipe(LogType.Warning, message);

        public void LogError(object message) => Pipe(LogType.Error, message);

        /// <summary>
        /// Pipes the specified message to the notification box.
        /// </summary>
        private void Pipe(LogType type, object message)
        {
            if (NotificationBox != null && type >= PipeLogLevel)
            {
                UnityThread.DispatchUnattended(() =>
                {
                    NotificationBox.Add(new Notification()
                    {
                        Message = message.ToString(),
                        Type = (NotificationType)type,
                    });
                    return null;
                });
            }
        }
    }
}