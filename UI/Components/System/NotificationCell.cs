using PBGame.UI.Components.Common;
using PBGame.Notifications;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.System
{
    public class NotificationCell : BaseNotificationCell, IRecyclable<NotificationCell> {

        /// <summary>
        /// The duration of message before it automatically hides.
        /// </summary>
        private const float ShowDuration = 4f;

        private float curDuration;


        public IRecycler<NotificationCell> Recycler { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnTriggered += () =>
            {
                Hide();
            };
        }

        public override void Show(INotification notification)
        {
            base.Show(notification);
            curDuration = ShowDuration;
        }

        protected override void Update()
        {
            if(IsAnimating)
                return;

            base.Update();

            // Handle auto hiding after certain time.
            if (curDuration > 0f)
            {
                curDuration -= Time.deltaTime;
                if (curDuration <= 0f)
                    Hide();
            }
        }
    }
}