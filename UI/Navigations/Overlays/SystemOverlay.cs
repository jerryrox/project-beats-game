using PBGame.UI.Models;
using PBGame.UI.Components.System;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class SystemOverlay : BaseOverlay<SystemModel> {

        /// <summary>
        /// Amount of default padding applied for inner display components from overlay rect.
        /// </summary>
        private const float DisplayerPadding = 12f;

        private NotificationDisplayer messageDisplayer;
        private FpsDisplayer fpsDisplayer;

        private IAnime menubarShowAni;
        private IAnime menubarHideAni;


        protected override int ViewDepth => ViewDepths.SystemOverlay;


        [InitWithDependency]
        private void Init()
        {
            fpsDisplayer = CreateChild<FpsDisplayer>("fps-displayer", 100);
            {
                fpsDisplayer.Anchor = AnchorType.BottomRight;
                fpsDisplayer.Pivot = PivotType.BottomRight;
                fpsDisplayer.Position = new Vector3(-DisplayerPadding, DisplayerPadding);
                fpsDisplayer.Size = new Vector2(170f, 30f);
            }
            messageDisplayer = CreateChild<NotificationDisplayer>("message-displayer", 1);
            {
                messageDisplayer.Anchor = AnchorType.TopRight;
                messageDisplayer.Pivot = PivotType.Right;
                messageDisplayer.Position = new Vector3(-DisplayerPadding, -DisplayerPadding);
                messageDisplayer.Size = new Vector2(320f, 0f);
            }

            menubarShowAni = new Anime();
            menubarShowAni.AnimateFloat(y => messageDisplayer.Y = y)
                .AddTime(0f, () => messageDisplayer.Y)
                .AddTime(0.25f, () => -DisplayerPadding - MenuBarHeight)
                .Build();

            menubarHideAni = new Anime();
            menubarHideAni.AnimateFloat(y => messageDisplayer.Y = y)
                .AddTime(0f, () => messageDisplayer.Y)
                .AddTime(0.25f, () => -DisplayerPadding)
                .Build();

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            model.IsFpsEnabled.BindAndTrigger(OnShowFpsChange);
            model.IsMenuBarActive.BindAndTrigger(OnMenuBarActivate);
            model.IsNotificationOverlayActive.BindAndTrigger(OnNotificationOverlayActivate);

            model.IsMessageEnabled.OnNewValue += OnDisplayMessagesChange;
            model.IsMessageEnabledGame.OnNewValue += OnDisplayMessagesGameChange;
            model.IsGameScreen.OnNewValue += OnGameScreen;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            model.IsFpsEnabled.Unbind(OnShowFpsChange);
            model.IsMenuBarActive.Unbind(OnMenuBarActivate);
            model.IsNotificationOverlayActive.Unbind(OnNotificationOverlayActivate);

            model.IsMessageEnabled.Unbind(OnDisplayMessagesChange);
            model.IsMessageEnabledGame.Unbind(OnDisplayMessagesGameChange);
            model.IsGameScreen.Unbind(OnGameScreen);
        }

        /// <summary>
        /// Adjusts display component positions based on menubar existence.
        /// </summary>
        private void AdjustForMenubar(bool hasMenubar)
        {
            menubarShowAni.Stop();
            menubarHideAni.Stop();

            if(hasMenubar)
                menubarShowAni.PlayFromStart();
            else
                menubarHideAni.PlayFromStart();
        }

        /// <summary>
        /// Toggles message displayer based on current state.
        /// </summary>
        private void ToggleMessageDisplayer()
        {
            bool isMessageEnbled = model.IsMessageEnabled.Value;
            if (isMessageEnbled)
            {
                // TODO: Hide when NotificationMenuOverlay is currently displayed.

                if (model.IsGameScreen.Value)
                {
                    messageDisplayer.ToggleDisplay(model.IsMessageEnabledGame.Value);
                    return;
                }
            }

            // Message displayer should be inactive when there is a notification menu overlay in view.
            bool isNotifOverlayEnabled = model.IsNotificationOverlayActive.Value;
            
            messageDisplayer.ToggleDisplay(isMessageEnbled && !isNotifOverlayEnabled);
        }

        /// <summary>
        /// Event called on show fps settings change.
        /// </summary>
        private void OnShowFpsChange(bool show) => fpsDisplayer.ToggleDisplay(show);

        /// <summary>
        /// Event called on display messages settings change.
        /// </summary>
        private void OnDisplayMessagesChange(bool show) => ToggleMessageDisplayer();
        
        /// <summary>
        /// Event called on display messages in game settings change.
        /// </summary>
        private void OnDisplayMessagesGameChange(bool show) => ToggleMessageDisplayer();

        /// <summary>
        /// Event called on menu bar overlay active state change.
        /// </summary>
        private void OnMenuBarActivate(bool isActive)
        {
            ToggleMessageDisplayer();
            AdjustForMenubar(isActive);
        }

        /// <summary>
        /// Event called on notification menu overlay active state change.
        /// </summary>
        private void OnNotificationOverlayActivate(bool isActive)
        {
            ToggleMessageDisplayer();
        }

        /// <summary>
        /// Event called when the game screen active state has changed.
        /// </summary>
        private void OnGameScreen(bool isActive) => ToggleMessageDisplayer();
    }
}