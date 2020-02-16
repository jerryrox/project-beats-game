using System;
using PBGame.UI.Navigations.Overlays;
using PBGame.Data.Users;
using PBGame.Assets.Caching;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Allocation.Caching;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MenuBar
{
    public class ProfileMenuButton : BaseMenuButton {

        private ISprite background;
        private ISprite imageBackground;
        private ITexture imageTexture;
        private ILabel nicknameLabel;
        private ILabel levelLabel;

        private bool hasOverlay = false;
        private CacherAgent<Texture2D> cacherAgent;


        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }

        [ReceivesDependency]
        private IWebImageCacher WebImageCacher { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init(IApiManager apiManager)
        {
            OnToggleOn += () =>
            {
                var overlay = OverlayNavigator.Show<ProfileMenuOverlay>();
                overlay.OnClose += () =>
                {
                    hasOverlay = false;
                    SetToggle(false);
                };
                hasOverlay = true;
            };
            OnToggleOff += () =>
            {
                if (hasOverlay)
                    OverlayNavigator.Hide<ProfileMenuOverlay>();
                hasOverlay = false;
            };

            cacherAgent = new CacherAgent<Texture2D>(WebImageCacher);
            cacherAgent.OnFinished += OnAvatarLoaded;

            background = CreateChild<UguiSprite>("background", -1);
            {
                background.Anchor = Anchors.Fill;
                background.RawSize = Vector2.zero;
                background.Color = new Color(0f, 0f, 0f, 0.125f);
            }
            imageBackground = CreateChild<UguiSprite>("image-bg", 5);
            {
                imageBackground.Anchor = Anchors.Left;
                imageBackground.Pivot = Pivots.Left;
                imageBackground.X = 8f;
                imageBackground.Size = new Vector2(48f, 48f);
                imageBackground.Color = new Color(0f, 0f, 0f, 0.125f);

                imageTexture = imageBackground.CreateChild<UguiTexture>("image", 5);
                {
                    imageTexture.Anchor = Anchors.Fill;
                    imageTexture.RawSize = Vector2.zero;
                    imageTexture.Position = Vector2.zero;
                    imageTexture.Active = false;
                }
            }
            nicknameLabel = CreateChild<Label>("nickname", 6);
            {
                nicknameLabel.Anchor = Anchors.Fill;
                nicknameLabel.Alignment = TextAnchor.MiddleLeft;
                nicknameLabel.OffsetLeft = 66f;
                nicknameLabel.OffsetRight = 10f;
                nicknameLabel.OffsetTop = 8f;
                nicknameLabel.OffsetBottom = 8f;
                nicknameLabel.WrapText = true;

            }
            levelLabel = CreateChild<Label>("level", 7);
            {
                levelLabel.Anchor = Anchors.Fill;
                levelLabel.Alignment = TextAnchor.LowerRight;
                levelLabel.OffsetLeft = 66f;
                levelLabel.OffsetRight = 10f;
                levelLabel.OffsetTop = 8f;
                levelLabel.OffsetBottom = 8f;
                levelLabel.FontSize = 14;
                levelLabel.WrapText = true;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            // Listen to online user change event.
            UserManager.CurrentUser.OnValueChanged += OnUserChange;
            OnUserChange(UserManager.CurrentUser.Value);
        }

        protected override void OnDisable()
        {
            // Withdraw from online user change event.
            UserManager.CurrentUser.OnValueChanged -= OnUserChange;
        }

        /// <summary>
        /// Event called from cacher agent when the avatar has been loaded.
        /// </summary>
        private void OnAvatarLoaded(Texture2D texture)
        {
            imageTexture.Active = true;
            imageTexture.Texture = texture;
        }

        /// <summary>
        /// Event called when the online user has changed.
        /// </summary>
        private void OnUserChange(IUser newUser, IUser _ = null)
        {
            imageTexture.Active = false;

            // Set infos
            if(newUser != null)
                nicknameLabel.Text = newUser.Username;
            else
                nicknameLabel.Text = ApiManager.OfflineUser.Username;

            // Unload profie image using web image cacher.
            cacherAgent.Remove();

            // Load profile image using web image cacher.
            if (newUser != null && !string.IsNullOrEmpty(newUser.OnlineUser.AvatarImage))
                cacherAgent.Request(newUser.OnlineUser.AvatarImage);
        }
    }
}