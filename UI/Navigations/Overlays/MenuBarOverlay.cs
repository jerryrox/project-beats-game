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

        private BaseMenuButton[] menuButtons;

        // TODO: Register more entries for more screen types.
        /// <summary>
        /// Table of colors mapped to screen types for automatic color adjustment.
        /// </summary>
        private Dictionary<Type, Color> backgroundColors = new Dictionary<Type, Color>()
        {
            { typeof(HomeScreen), new Color(0f, 0f, 0f, 0f) },
            { typeof(SongsScreen), new Color(0f, 0f, 0f, 0f) }
        };


        public float ContainerHeight => 64f;

        public BackgroundSprite BackgroundSprite { get; private set; }

        public BaseMenuButton ComboMenuButton { get; private set; }

        public BaseMenuButton ProfileButton { get; private set; }

        public MusicButton MusicButton { get; private set; }

        public BaseMenuButton SettingsMenuButton { get; private set; }

        public BaseMenuButton NotificationMenuButton { get; private set; }

        protected override int OverlayDepth => ViewDepths.MenuBarOverlay;

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            container = CreateChild<UguiObject>("container");
            {
                container.Anchor = AnchorType.TopStretch;
                container.Pivot = PivotType.Top;
                container.SetOffsetHorizontal(0f);
                container.Height = ContainerHeight;
                container.Y = 0f;

                BackgroundSprite = container.CreateChild<BackgroundSprite>("background");
                {
                    BackgroundSprite.Anchor = AnchorType.Fill;
                    BackgroundSprite.RawSize = Vector2.zero;
                    BackgroundSprite.SetOffsetVertical(0f);
                }
                ComboMenuButton = container.CreateChild<ComboMenuButton>("combo-menu", 1);
                {
                    ComboMenuButton.Anchor = AnchorType.LeftStretch;
                    ComboMenuButton.Pivot = PivotType.Left;
                    ComboMenuButton.SetOffsetVertical(0f);
                    ComboMenuButton.X = 0f;
                    ComboMenuButton.Width = 80f;
                }
                ProfileButton = container.CreateChild<ProfileMenuButton>("profile-menu", 2);
                {
                    ProfileButton.Anchor = AnchorType.LeftStretch;
                    ProfileButton.Pivot = PivotType.Left;
                    ProfileButton.SetOffsetVertical(0f);
                    ProfileButton.X = 80f;
                    ProfileButton.Width = 220f;
                }
                MusicButton = container.CreateChild<MusicButton>("music", 3);
                {
                    MusicButton.Anchor = AnchorType.RightStretch;
                    MusicButton.Pivot = PivotType.Right;
                    MusicButton.SetOffsetVertical(0f);
                    MusicButton.X = -160f;
                    MusicButton.Width = 80f;
                }
                SettingsMenuButton = container.CreateChild<SettingsMenuButton>("settings-menu", 4);
                {
                    SettingsMenuButton.Anchor = AnchorType.RightStretch;
                    SettingsMenuButton.Pivot = PivotType.Right;
                    SettingsMenuButton.SetOffsetVertical(0f);
                    SettingsMenuButton.X = -80f;
                    SettingsMenuButton.Width = 80f;
                }
                NotificationMenuButton = container.CreateChild<NotificationMenuButton>("notification-menu", 5);
                {
                    NotificationMenuButton.Anchor = AnchorType.RightStretch;
                    NotificationMenuButton.Pivot = PivotType.Right;
                    NotificationMenuButton.SetOffsetVertical(0f);
                    NotificationMenuButton.X = 0f;
                    NotificationMenuButton.Width = 80f;
                }
            }

            menuButtons = GetComponentsInChildren<BaseMenuButton>(true);
            HookMenuButtonFocus();

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
        /// Listens to all menu buttons' focus events to automatically unfocus other menu buttons.
        /// </summary>
        private void HookMenuButtonFocus()
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                var menu = menuButtons[i];
                menu.OnFocused += (isFocused) =>
                {
                    if (isFocused)
                        UnfocusAllMenu(menu);
                };
            }
        }

        /// <summary>
        /// Unfocuses all menu buttons except the specified button, if specified.
        /// </summary>
        private void UnfocusAllMenu(BaseMenuButton exception)
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                if(menuButtons[i] != exception)
                    menuButtons[i].IsFocused = false;
            }
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