using PBGame.Data.Users;
using PBGame.Assets.Caching;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Allocation.Caching;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    // TODO: Support for logging in using other API providers.
    public class CoverDisplay : UguiObject, ICoverDisplay
    {
        private ITexture image;

        private CacherAgent<Texture2D> imageAgent;


        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

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
            UserManager.CurrentUser.OnValueChanged += OnUserChange;

            OnUserChange(UserManager.CurrentUser.Value);
        }

        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            UserManager.CurrentUser.OnValueChanged -= OnUserChange;
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
        private void OnUserChange(IUser user, IUser _ = null)
        {
            RemoveImage();
            if(user != null && !string.IsNullOrEmpty(user.OnlineUser.CoverImage))
                imageAgent.Request(user.OnlineUser.CoverImage);
        }
    }
}