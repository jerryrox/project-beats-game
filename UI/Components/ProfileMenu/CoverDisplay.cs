using PBGame.Assets.Caching;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Allocation.Caching;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class CoverDisplay : UguiObject, ICoverDisplay
    {
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

            image = CreateChild<UguiTexture>("image", 0);
            {
                image.Anchor = Anchors.Fill;
                image.RawSize = Vector2.zero;
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
            if(!string.IsNullOrEmpty(user.CoverImage))
                imageAgent.Request(user.CoverImage);
        }
    }
}