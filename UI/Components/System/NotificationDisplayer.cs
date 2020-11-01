using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Notifications;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.System
{
    public class NotificationDisplayer : UguiObject, IDisplayer
    {

        private NotificationList notificationList;
        private CanvasGroup canvasGroup;

        private IAnime showAni;
        private IAnime hideAni;

        [ReceivesDependency]
        private SystemModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            notificationList = CreateChild<NotificationList>("notification-list");
            {
                notificationList.Anchor = AnchorType.Fill;
                notificationList.Offset = Offset.Zero;
                notificationList.Scope = NotificationScope.Temporary;

                notificationList.OnDismiss += OnDismissed;
            }

            showAni = new Anime();
            showAni.AddEvent(0f, () => Active = true);
            showAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, () => canvasGroup.alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, () => canvasGroup.alpha)
                .AddTime(0.25f, 1f)
                .Build();
            hideAni.AddEvent(hideAni.Duration, () => Active = false);

            OnEnableInited();
        }

        public void ToggleDisplay(bool enable)
        {
            if((enable && showAni.IsPlaying) ||
                (!enable && hideAni.IsPlaying))
                return;

            showAni.Stop();
            hideAni.Stop();

            if (enable)
                showAni.PlayFromStart();
            else
                hideAni.PlayFromStart();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.OnNewNotification += OnNotification;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.OnNewNotification -= OnNotification;
        }

        /// <summary>
        /// Event called when a new notification is received.
        /// </summary>
        private void OnNotification(INotification notification)
        {
            notificationList.DisplayNotification(notification);
        }

        /// <summary>
        /// Event called when a notification has been dismissed from the list.
        /// </summary>
        private void OnDismissed(INotification notification)
        {
            Model.RemoveNotification(notification);
        }
    }
}