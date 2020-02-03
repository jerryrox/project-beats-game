using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.MenuBar;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class MenuBarOverlay : BaseOverlay, IMenuBarOverlay {

        private UguiObject container;

        public IBackgroundSprite BackgroundSprite { get; private set; }

        public IMenuButton ComboMenuButton { get; private set; }

        public IMenuButton ProfileButton { get; private set; }

        public IMenuButton SettingsMenuButton { get; private set; }

        public IMenuButton NotificationMenuButton { get; private set; }

        protected override int OverlayDepth => ViewDepths.MenuBarOverlay;


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
                SettingsMenuButton = container.CreateChild<SettingsMenuButton>("settings-menu", 3);
                {
                    SettingsMenuButton.Anchor = Anchors.RightStretch;
                    SettingsMenuButton.Pivot = Pivots.Right;
                    SettingsMenuButton.OffsetTop = 0;
                    SettingsMenuButton.OffsetBottom = 0;
                    SettingsMenuButton.X = -80f;
                    SettingsMenuButton.Width = 80f;
                }
                NotificationMenuButton = container.CreateChild<NotificationMenuButton>("notification-menu", 4);
                {
                    NotificationMenuButton.Anchor = Anchors.RightStretch;
                    NotificationMenuButton.Pivot = Pivots.Right;
                    NotificationMenuButton.OffsetTop = 0;
                    NotificationMenuButton.OffsetBottom = 0;
                    NotificationMenuButton.X = 0f;
                    NotificationMenuButton.Width = 80f;
                }
            }
        }
    }
}