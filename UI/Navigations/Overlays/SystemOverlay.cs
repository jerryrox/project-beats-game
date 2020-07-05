using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.System;
using PBGame.UI.Navigations.Screens;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class SystemOverlay : BaseOverlay, ISystemOverlay {

        /// <summary>
        /// Amount of default padding applied for inner display components from overlay rect.
        /// </summary>
        private const float DisplayerPadding = 12f;

        private IAnime menubarShowAni;
        private IAnime menubarHideAni;


        public MessageDisplayer MessageDisplayer { get; private set; }

        public FpsDisplayer FpsDisplayer { get; private set; }

        protected override int ViewDepth => ViewDepths.SystemOverlay;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


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
            GameConfiguration.ShowFps.BindAndTrigger(OnShowFpsChange);
            GameConfiguration.DisplayMessages.BindAndTrigger(OnDisplayMessagesChange);

            OverlayNavigator.OnShowView += OnOverlayShow;
            OverlayNavigator.OnHideView += OnOverlayHide;

            ScreenNavigator.OnShowView += OnScreenShow;

            AdjustForMenubar(OverlayNavigator.IsActive(typeof(MenuBarOverlay)));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameConfiguration.ShowFps.OnNewValue -= OnShowFpsChange;
            GameConfiguration.DisplayMessages.OnNewValue -= OnDisplayMessagesChange;

            OverlayNavigator.OnShowView -= OnOverlayShow;
            OverlayNavigator.OnHideView -= OnOverlayHide;

            ScreenNavigator.OnShowView -= OnScreenShow;
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
            if (GameConfiguration.DisplayMessages.Value)
            {
                // TODO: Hide when NotificationMenuOverlay is currently displayed.

                // TODO: Uncomment when GameScreen is implemented.
                // if (ScreenNavigator.CurrentScreen is GameScreen)
                //     MessageDisplayer.ToggleDisplay(GameConfiguration.DisplayMessagesInGame.Value);
                // else
                MessageDisplayer.ToggleDisplay(true);
            }
            else
            {
                MessageDisplayer.ToggleDisplay(false);
            }
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
        /// Event called on overlay show.
        /// </summary>
        private void OnOverlayShow(INavigationView view)
        {
            ToggleMessageDisplayer();
            if(view is MenuBarOverlay)
                AdjustForMenubar(true);
        }

        /// <summary>
        /// Event called on overlay hide.
        /// </summary>
        private void OnOverlayHide(INavigationView view)
        {
            ToggleMessageDisplayer();
            if(view is MenuBarOverlay)
                AdjustForMenubar(false);
        }

        /// <summary>
        /// Event called on screen show.
        /// </summary>
        private void OnScreenShow(INavigationView view)
        {
            ToggleMessageDisplayer();
        }
    }
}