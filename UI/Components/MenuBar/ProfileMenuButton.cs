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


        protected override string IconSpritename => "";

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
            OnFocused += (isFocused) =>
            {
                if (isFocused)
                {
                    var overlay = OverlayNavigator.Show<ProfileMenuOverlay>();
                    overlay.OnClose += () =>
                    {
                        hasOverlay = false;
                        IsFocused = false;
                    };
                    hasOverlay = true;
                }
                else
                {
                    if (hasOverlay)
                        OverlayNavigator.Hide<ProfileMenuOverlay>();
                    hasOverlay = false;
                }
            };

            cacherAgent = new CacherAgent<Texture2D>(WebImageCacher);
            cacherAgent.OnFinished += OnAvatarLoaded;

            background = CreateChild<UguiSprite>("background", -1);
            {
                background.Anchor = AnchorType.Fill;
                background.RawSize = Vector2.zero;
                background.Color = new Color(0f, 0f, 0f, 0.125f);
            }
            imageBackground = CreateChild<UguiSprite>("image-bg", 5);
            {
                imageBackground.Anchor = AnchorType.Left;
                imageBackground.Pivot = PivotType.Left;
                imageBackground.X = 8f;
                imageBackground.Size = new Vector2(48f, 48f);
                imageBackground.Color = new Color(0f, 0f, 0f, 0.125f);

                imageTexture = imageBackground.CreateChild<UguiTexture>("image", 5);
                {
                    imageTexture.Anchor = AnchorType.Fill;
                    imageTexture.RawSize = Vector2.zero;
                    imageTexture.Position = Vector2.zero;
                    imageTexture.Active = false;
                }
            }
            nicknameLabel = CreateChild<Label>("nickname", 6);
            {
                nicknameLabel.Anchor = AnchorType.Fill;
                nicknameLabel.Alignment = TextAnchor.MiddleLeft;
                nicknameLabel.Offset = new Offset(66f, 8f, 10f, 8f);
                nicknameLabel.WrapText = true;

            }
            levelLabel = CreateChild<Label>("level", 7);
            {
                levelLabel.Anchor = AnchorType.Fill;
                levelLabel.Alignment = TextAnchor.LowerRight;
                levelLabel.Offset = new Offset(66f, 8f, 10f, 8f);
                levelLabel.FontSize = 14;
                levelLabel.WrapText = true;
            }

            // No need for icon.
            iconSprite.Destroy();

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