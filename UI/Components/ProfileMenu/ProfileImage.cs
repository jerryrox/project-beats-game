using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Assets.Caching;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Allocation.Caching;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    // TODO: Support for logging in using other API providers.
    public class ProfileImage : UguiObject, IProfileImage {

        private ISprite glow;
        private ISprite mask;
        private ITexture image;

        private CacherAgent<Texture2D> imageAgent;


        /// <summary>
        /// Returns the osu api from manager.
        /// </summary>
        private IApi OsuApi => ApiManager.GetApi(ApiProviders.Osu);

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }

        [ReceivesDependency]
        private IWebImageCacher WebImageCacher { get; set; }


        [InitWithDependency]
        private void Init()
        {
            imageAgent = new CacherAgent<Texture2D>(WebImageCacher);
            imageAgent.OnFinished += (profileImage) =>
            {
                image.Texture = profileImage;
                image.Active = profileImage != null;
            };

            glow = CreateChild<UguiSprite>("glow", 0);
            {
                glow.Anchor = Anchors.Fill;
                glow.RawSize = new Vector2(-60f, -60f);
                glow.SpriteName = "glow-circle-32";
                glow.Color = new Color(0f, 0f, 0f, 0.5f);
            }
            mask = CreateChild<UguiSprite>("mask", 1);
            {
                mask.Anchor = Anchors.Fill;
                mask.RawSize = Vector2.zero;
                mask.Color = new Color(0.125f, 0.125f, 0.125f);
                mask.SpriteName = "circle-320";

                mask.AddEffect(new MaskEffect());

                image = mask.CreateChild<UguiTexture>("image", 2);
                {
                    image.Anchor = Anchors.Fill;
                    image.RawSize = Vector2.zero;
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
            RemoveImage();
        }

        /// <summary>
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            OsuApi.User.OnValueChanged += OnUserChange;

            OnUserChange(OsuApi.User.Value);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            OsuApi.User.OnValueChanged -= OnUserChange;
        }

        /// <summary>
        /// Removes profile image texture from image.
        /// </summary>
        private void RemoveImage()
        {
            imageAgent.Remove();
            image.Active = false;
        }

        /// <summary>
        /// Event called when the online user has changed.
        /// </summary>
        private void OnUserChange(IOnlineUser user, IOnlineUser _ = null)
        {
            RemoveImage();
            if (!string.IsNullOrEmpty(user.AvatarImage))
                imageAgent.Request(user.AvatarImage);
        }
    }
}