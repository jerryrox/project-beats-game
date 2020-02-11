using System;
using System.Collections.Generic;
using PBGame.UI.Components.MenuBar;
using PBGame.UI.Navigations.Screens;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class MenuBarOverlay : BaseOverlay, IMenuBarOverlay {
        
        private UguiObject container;

        // TODO: Register more entries for more screen types.
        /// <summary>
        /// Table of colors mapped to screen types for automatic color adjustment.
        /// </summary>
        private Dictionary<Type, Color> backgroundColors = new Dictionary<Type, Color>()
        {
            { typeof(HomeScreen), new Color(0f, 0f, 0f, 0f) },
            { typeof(SongsScreen), new Color(0f, 0f, 0f, 0f) }
        };


        public IBackgroundSprite BackgroundSprite { get; private set; }

        public IMenuButton ComboMenuButton { get; private set; }

        public IMenuButton ProfileButton { get; private set; }

        public IMusicButton MusicButton { get; private set; }

        public IMenuButton SettingsMenuButton { get; private set; }

        public IMenuButton NotificationMenuButton { get; private set; }

        protected override int OverlayDepth => ViewDepths.MenuBarOverlay;

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            container = CreateChild<UguiObject>("container");
            {
                container.Anchor = Anchors.TopStretch;
                container.Pivot = Pivots.Top;
                container.OffsetLeft = 0f;
                container.OffsetRight = 0f;
                container.Height = 64f;
                container.Y = 0f;

                BackgroundSprite = container.CreateChild<BackgroundSprite>("background");
                {
                    BackgroundSprite.Anchor = Anchors.Fill;
                    BackgroundSprite.RawSize = Vector2.zero;
                    BackgroundSprite.OffsetTop = 0f;
                    BackgroundSprite.OffsetBottom = 0f;
                }
                ComboMenuButton = container.CreateChild<ComboMenuButton>("combo-menu", 1);
                {
                    ComboMenuButton.Anchor = Anchors.LeftStretch;
                    ComboMenuButton.Pivot = Pivots.Left;
                    ComboMenuButton.OffsetTop = 0;
                    ComboMenuButton.OffsetBottom = 0;
                    ComboMenuButton.X = 0f;
                    ComboMenuButton.Width = 80f;
                }
                ProfileButton = container.CreateChild<ProfileMenuButton>("profile-menu", 2);
                {
                    ProfileButton.Anchor = Anchors.LeftStretch;
                    ProfileButton.Pivot = Pivots.Left;
                    ProfileButton.OffsetTop = 0;
                    ProfileButton.OffsetBottom = 0;
                    ProfileButton.X = 80f;
                    ProfileButton.Width = 220f;
                }
                MusicButton = container.CreateChild<MusicButton>("music", 3);
                {
                    MusicButton.Anchor = Anchors.RightStretch;
                    MusicButton.Pivot = Pivots.Right;
                    MusicButton.OffsetTop = 0f;
                    MusicButton.OffsetBottom = 0f;
                    MusicButton.X = -160f;
                    MusicButton.Width = 80f;
                }
                SettingsMenuButton = container.CreateChild<SettingsMenuButton>("settings-menu", 4);
                {
                    SettingsMenuButton.Anchor = Anchors.RightStretch;
                    SettingsMenuButton.Pivot = Pivots.Right;
                    SettingsMenuButton.OffsetTop = 0;
                    SettingsMenuButton.OffsetBottom = 0;
                    SettingsMenuButton.X = -80f;
                    SettingsMenuButton.Width = 80f;
                }
                NotificationMenuButton = container.CreateChild<NotificationMenuButton>("notification-menu", 5);
                {
                    NotificationMenuButton.Anchor = Anchors.RightStretch;
                    NotificationMenuButton.Pivot = Pivots.Right;
                    NotificationMenuButton.OffsetTop = 0;
                    NotificationMenuButton.OffsetBottom = 0;
                    NotificationMenuButton.X = 0f;
                    NotificationMenuButton.Width = 80f;
                }
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            BindEvents();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            UnbindEvents();
        }

        /// <summary>
        /// Binds to screen change events.
        /// </summary>
        private void BindEvents()
        {
            ScreenNavigator.OnShowView += OnScreenChange;

            OnScreenChange(ScreenNavigator.CurrentScreen);
        }

        /// <summary>
        /// Unbinds from screen change events.
        /// </summary>
        private void UnbindEvents()
        {
            ScreenNavigator.OnShowView -= OnScreenChange;
        }

        /// <summary>
        /// Event called when current screen has changed.
        /// </summary>
        private void OnScreenChange(INavigationView screen)
        {
            MusicButton.Active = screen is HomeScreen;

            if (backgroundColors.TryGetValue(screen.GetType(), out Color color))
                BackgroundSprite.Color = color;
        }
    }
}