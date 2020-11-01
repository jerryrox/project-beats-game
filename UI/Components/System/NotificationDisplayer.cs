using PBGame.UI.Components.Common;
using PBGame.Notifications;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.System
{
    public class NotificationDisplayer : UguiObject, IDisplayer {

        private NotificationList notificationList;
        private CanvasGroup canvasGroup;

        private IAnime showAni;
        private IAnime hideAni;


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            notificationList = CreateChild<NotificationList>("notification-list");
            {
                notificationList.Anchor = AnchorType.Fill;
                notificationList.Offset = Offset.Zero;

                notificationList.Scope = NotificationScope.Temporary;
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
        }

        public void ToggleDisplay(bool enable)
        {
            showAni.Stop();
            hideAni.Stop();

            if(enable)
                showAni.PlayFromStart();
            else
                hideAni.PlayFromStart();
        }
    }
}