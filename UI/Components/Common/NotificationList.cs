using PBGame.UI.Models;
using PBGame.Notifications;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    public class NotificationList : UguiObject {

        private ManagedRecycler<NotificationCell> cellRecycler;

        private CanvasGroup canvasGroup;


        // TODO: Assign this when creating this list.
        /// <summary>
        /// The displayal scope of the notifications.
        /// </summary>
        public NotificationScope Scope { get; set; }

        [ReceivesDependency]
        private SystemModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            cellRecycler = new ManagedRecycler<NotificationCell>(CreateCell);
            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            OnEnableInited();
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

            cellRecycler.ReturnAll();
        }

        /// <summary>
        /// Displays the specified notification.
        /// </summary>
        public void DisplayNotification(INotification notification)
        {
            // Display a new cell for this notification
            var cell = cellRecycler.GetNext();
            cell.Show(notification, Scope);
            cell.PositionTo(GetNextCellPos(), false);
        }

        /// <summary>
        /// Returns the Y position for the next cell.
        /// </summary>
        private float GetNextCellPos()
        {
            if(cellRecycler.ActiveCount <= 1)
                return 0f;
            var lastCell = cellRecycler.ActiveObjects[cellRecycler.ActiveObjects.Count - 2];
            return lastCell.TargetY - lastCell.Height;
        }

        /// <summary>
        /// Adjusts all active cells' Y positions in case a message was removed from anywhere except the beginning.
        /// </summary>
        private void AdjustCellPos()
        {
            float nextPos = 0f;
            foreach (var cell in cellRecycler.ActiveObjects)
            {
                cell.PositionTo(nextPos, true);
                nextPos -= cell.Height;
            }
        }

        /// <summary>
        /// Creates a new message cell.
        /// </summary>
        private NotificationCell CreateCell()
        {
            var cell = CreateChild<NotificationCell>("cell", ChildCount);
            cell.Anchor = AnchorType.Top;
            cell.Pivot = PivotType.Top;
            cell.Width = this.Width;
            cell.OnHidden += (c) => {
                // Remove from notifications automatically if hidden.
                Model.RemoveNotification(cell.Notification);
                cellRecycler.Return(cell);
                AdjustCellPos();
            };
            return cell;
        }

        /// <summary>
        /// Event called on new notification.
        /// </summary>
        private void OnNotification(INotification notification)
        {
            DisplayNotification(notification);
        }
    }
}