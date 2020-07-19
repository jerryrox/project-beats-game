using PBGame.UI.Models;
using PBGame.UI.Components.MenuBar;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class MenuBarOverlay : BaseOverlay<MenuBarModel> {
        
        private UguiObject container;

        private BackgroundSprite backgroundSprite;
        private ComboMenuButton comboMenuButton;
        private ProfileMenuButton profileButton;
        private MusicButton musicButton;
        private SettingsMenuButton settingsMenuButton;
        private NotificationMenuButton notificationMenuButton;
        private BaseMenuButton[] menuButtons;


        /// <summary>
        /// Returns the height of the container.
        /// </summary>
        public float ContainerHeight => 64f;

        protected override int ViewDepth => ViewDepths.MenuBarOverlay;


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

                backgroundSprite = container.CreateChild<BackgroundSprite>("background");
                {
                    backgroundSprite.Anchor = AnchorType.Fill;
                    backgroundSprite.RawSize = Vector2.zero;
                    backgroundSprite.SetOffsetVertical(0f);
                }
                comboMenuButton = container.CreateChild<ComboMenuButton>("combo-menu", 1);
                {
                    comboMenuButton.Anchor = AnchorType.LeftStretch;
                    comboMenuButton.Pivot = PivotType.Left;
                    comboMenuButton.SetOffsetVertical(0f);
                    comboMenuButton.X = 0f;
                    comboMenuButton.Width = 80f;
                }
                profileButton = container.CreateChild<ProfileMenuButton>("profile-menu", 2);
                {
                    profileButton.Anchor = AnchorType.LeftStretch;
                    profileButton.Pivot = PivotType.Left;
                    profileButton.SetOffsetVertical(0f);
                    profileButton.X = 80f;
                    profileButton.Width = 220f;
                }
                musicButton = container.CreateChild<MusicButton>("music", 3);
                {
                    musicButton.Anchor = AnchorType.RightStretch;
                    musicButton.Pivot = PivotType.Right;
                    musicButton.SetOffsetVertical(0f);
                    musicButton.X = -160f;
                    musicButton.Width = 80f;
                }
                settingsMenuButton = container.CreateChild<SettingsMenuButton>("settings-menu", 4);
                {
                    settingsMenuButton.Anchor = AnchorType.RightStretch;
                    settingsMenuButton.Pivot = PivotType.Right;
                    settingsMenuButton.SetOffsetVertical(0f);
                    settingsMenuButton.X = -80f;
                    settingsMenuButton.Width = 80f;
                }
                notificationMenuButton = container.CreateChild<NotificationMenuButton>("notification-menu", 5);
                {
                    notificationMenuButton.Anchor = AnchorType.RightStretch;
                    notificationMenuButton.Pivot = PivotType.Right;
                    notificationMenuButton.SetOffsetVertical(0f);
                    notificationMenuButton.X = 0f;
                    notificationMenuButton.Width = 80f;
                }
            }

            menuButtons = GetComponentsInChildren<BaseMenuButton>(true);

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.IsMusicButtonActive.BindAndTrigger(OnMusicButtonChange);
            Model.BarColor.BindAndTrigger(OnBarColorChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.IsMusicButtonActive.OnNewValue -= OnMusicButtonChange;
            model.BarColor.OnNewValue -= OnBarColorChange;
        }

        /// <summary>
        /// Event called when the music button's active state should change.
        /// </summary>
        private void OnMusicButtonChange(bool isActive)
        {
            musicButton.Active = isActive;
        }

        /// <summary>
        /// Event called on bar background color change.
        /// </summary>
        private void OnBarColorChange(Color color)
        {
            backgroundSprite.Color = color;
        }
    }
}