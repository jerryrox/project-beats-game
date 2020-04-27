using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.System
{
    public class MessageDisplayer : UguiObject, IDisplayer {

        private ManagedRecycler<MessageCell> cellRecycler;

        private CanvasGroup canvasGroup;

        private IAnime showAni;
        private IAnime hideAni;


        [InitWithDependency]
        private void Init()
        {
            cellRecycler = new ManagedRecycler<MessageCell>(CreateCell);
            canvasGroup = RawObject.AddComponent<CanvasGroup>();

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

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            
            // TODO: Listen to notifications
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            cellRecycler.ReturnAll();

            // TODO: Unsubscribe from notifications.
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

        /// <summary>
        /// Creates a new message cell.
        /// </summary>
        private MessageCell CreateCell()
        {
            var cell = CreateChild<MessageCell>("cell", ChildCount);
            cell.Anchor = Anchors.TopStretch;
            cell.Pivot = Pivots.Top;
            return cell;
        }

        /// <summary>
        /// Event called on new notification.
        /// </summary>
        private void OnNotification()
        {
            // TODO:
        }
    }
}