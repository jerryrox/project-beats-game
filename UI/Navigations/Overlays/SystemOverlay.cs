using PBGame.UI.Models;
using PBGame.UI.Components.System;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class SystemOverlay : BaseOverlay<SystemModel>, ISystemOverlay {

        /// <summary>
        /// Amount of default padding applied for inner display components from overlay rect.
        /// </summary>
        private const float DisplayerPadding = 12f;

        private IAnime menubarShowAni;
        private IAnime menubarHideAni;


        public MessageDisplayer MessageDisplayer { get; private set; }

        public FpsDisplayer FpsDisplayer { get; private set; }

        protected override int ViewDepth => ViewDepths.SystemOverlay;


        [InitWithDependency]
        private void Init()
        {
            FpsDisplayer = CreateChild<FpsDisplayer>("fps-displayer", 100);
            {
                FpsDisplayer.Anchor = AnchorType.BottomRight;
                FpsDisplayer.Pivot = PivotType.BottomRight;
                FpsDisplayer.Position = new Vector3(-DisplayerPadding, DisplayerPadding);
                FpsDisplayer.Size = new Vector2(170f, 30f);
            }
            MessageDisplayer = CreateChild<MessageDisplayer>("message-displayer", 1);
            {
                MessageDisplayer.Anchor = AnchorType.TopRight;
                MessageDisplayer.Pivot = PivotType.Right;
                MessageDisplayer.Position = new Vector3(-DisplayerPadding, -DisplayerPadding);
                MessageDisplayer.Size = new Vector2(320f, 0f);
            }

            menubarShowAni = new Anime();
            menubarShowAni.AnimateFloat(y => MessageDisplayer.Y = y)
                .AddTime(0f, () => MessageDisplayer.Y)
                .AddTime(0.25f, () => -DisplayerPadding - MenuBarHeight)
                .Build();

            menubarHideAni = new Anime();
            menubarHideAni.AnimateFloat(y => MessageDisplayer.Y = y)
                .AddTime(0f, () => MessageDisplayer.Y)
                .AddTime(0.25f, () => -DisplayerPadding)
                .Build();

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            model.IsFpsEnabled.BindAndTrigger(OnShowFpsChange);
            model.IsMenuBarActive.BindAndTrigger(OnMenuBarActivate);
            model.IsMessageEnabled.OnNewValue += OnDisplayMessagesChange;
            model.IsMessageEnabledGame.OnNewValue += OnDisplayMessagesGameChange;
            model.IsGameScreen.OnNewValue += OnGameScreen;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            model.IsFpsEnabled.OnNewValue -= OnShowFpsChange;
            model.IsMenuBarActive.OnNewValue -= OnMenuBarActivate;
            model.IsMessageEnabled.OnNewValue -= OnDisplayMessagesChange;
            model.IsMessageEnabledGame.OnNewValue -= OnDisplayMessagesGameChange;
            model.IsGameScreen.OnNewValue -= OnGameScreen;
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
                    MessageDisplayer.ToggleDisplay(model.IsMessageEnabledGame.Value);
                    return;
                }
            }

            MessageDisplayer.ToggleDisplay(isMessageEnbled);
        }

        /// <summary>
        /// Event called on show fps settings change.
        /// </summary>
        private void OnShowFpsChange(bool show) => FpsDisplayer.ToggleDisplay(show);

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
        /// Event called when the game screen active state has changed.
        /// </summary>
        private void OnGameScreen(bool isActive) => ToggleMessageDisplayer();
    }
}