using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.System;
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

        protected override int OverlayDepth => ViewDepths.SystemOverlay;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            FpsDisplayer = CreateChild<FpsDisplayer>("fps-displayer", 100);
            {
                FpsDisplayer.Anchor = Anchors.BottomRight;
                FpsDisplayer.Pivot = Pivots.BottomRight;
                FpsDisplayer.Position = new Vector3(-DisplayerPadding, DisplayerPadding);
                FpsDisplayer.Size = new Vector2(170f, 30f);

                GameConfiguration.ShowFps.BindAndTrigger((showFps, _) => FpsDisplayer.ToggleDisplay(showFps));
            }
            MessageDisplayer = CreateChild<MessageDisplayer>("message-displayer", 1);
            {
                MessageDisplayer.Anchor = Anchors.TopRight;
                MessageDisplayer.Pivot = Pivots.Right;
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
            OverlayNavigator.OnShowView += OnOverlayShow;
            OverlayNavigator.OnHideView += OnOverlayHide;

            AdjustForMenubar(OverlayNavigator.IsActive(typeof(MenuBarOverlay)));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OverlayNavigator.OnShowView -= OnOverlayShow;
            OverlayNavigator.OnHideView -= OnOverlayHide;
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
        /// Event called on overlay show.
        /// </summary>
        private void OnOverlayShow(INavigationView view)
        {
            if(view is MenuBarOverlay)
                AdjustForMenubar(true);
        }

        /// <summary>
        /// Event called on overlay hide.
        /// </summary>
        private void OnOverlayHide(INavigationView view)
        {
            if(view is MenuBarOverlay)
                AdjustForMenubar(false);
        }
    }
}