using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Notifications;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Overlays
{
    public class NotificationMenuOverlay : BaseSubMenuOverlay<NotificationMenuModel>
    {
        private NotificationList notificationList;
        private IScrollView scrollView;


        public override bool UseGlow => false;

        protected override int ViewDepth => ViewDepths.NotificationMenuOverlay;


        [InitWithDependency]
        private void Init()
        {
            container.Anchor = AnchorType.RightStretch;
            container.Pivot = PivotType.TopRight;
            container.X = 0f;
            container.Width = 340f;
            container.SetOffsetVertical(0f);

            scrollView = container.CreateChild<UguiScrollView>("scrollview");
            {
                scrollView.Anchor = AnchorType.Fill;
                scrollView.Offset = Offset.Zero;
                scrollView.Background.Color = new Color(1f, 1f, 1f, 0.0625f);

                notificationList = scrollView.Container.CreateChild<NotificationList>("list");
                {
                    notificationList.Anchor = AnchorType.Fill;
                    notificationList.Offset = Offset.Zero;
                    notificationList.Scope = NotificationScope.Stored;

                    notificationList.OnDismiss += OnDismissed;
                }
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.OnNewNotification += OnNewNotification;
            Model.OnRemoveNotification += OnRemoveNotification;

            DisplayAllNotifications();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.OnNewNotification -= OnNewNotification;
            Model.OnRemoveNotification -= OnRemoveNotification;
        }

        protected void Update()
        {
            if (scrollView != null)
            {
                scrollView.Container.Height = notificationList.DesiredHeight;
            }
        }

        /// <summary>
        /// Displays all notifications in the notification box.
        /// </summary>
        private void DisplayAllNotifications()
        {
            foreach (var notification in model.Notifications)
                notificationList.DisplayNotification(notification);
        }

        /// <summary>
        /// Event called when the specified notification was dismissed from the list.
        /// </summary>
        private void OnDismissed(INotification notification)
        {
            Model.RemoveNotification(notification);
        }

        /// <summary>
        /// Event called when a new notification is added.
        /// </summary>
        private void OnNewNotification(INotification notification)
        {
            notificationList.DisplayNotification(notification);
        }

        /// <summary>
        /// Event called when a notification is removed.
        /// </summary>
        private void OnRemoveNotification(INotification notification)
        {
            notificationList.HideNotification(notification);
        }
    }
}