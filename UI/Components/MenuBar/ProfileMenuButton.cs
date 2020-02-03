using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Assets.Caching;
using PBGame.Networking.API;
using PBFramework.UI;
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

        private CacherAgent<Texture2D> cacherAgent;


        /// <summary>
        /// Returns the osu api instance.
        /// </summary>
        private IApi OsuApi => ApiManager.GetApi(ApiProviders.Osu);

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }

        [ReceivesDependency]
        private IWebImageCacher WebImageCacher { get; set; }


        [InitWithDependency]
        private void Init(IApiManager apiManager)
        {
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

                imageTexture = CreateChild<UguiTexture>("image", 5);
                {
                    imageTexture.Anchor = Anchors.Fill;
                    imageTexture.RawSize = Vector2.zero;
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
            OsuApi.User.OnValueChanged += OnUserChange;
            OnUserChange(OsuApi.User.Value, null);
        }

        protected override void OnDisable()
        {
            // Withdraw from online user change event.
            OsuApi.User.OnValueChanged -= OnUserChange;
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
        private void OnUserChange(IOnlineUser newUser, IOnlineUser oldUser)
        {
            imageTexture.Active = false;

            // Set infos
            nicknameLabel.Text = newUser.Username;

            // Unload profie image using web image cacher.
            cacherAgent.Remove();

            if (OsuApi.IsOnline.Value)
            {
                // Load profile image using web image cacher.
                if(newUser.AvatarImage.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    cacherAgent.Request(newUser.AvatarImage);
            }
        }
    }
}