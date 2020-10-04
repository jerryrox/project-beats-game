using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Data.Users;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    public class ProfileImage : UguiObject {

        private ISprite glow;
        private AvatarDisplay avatarDisplay;


        [ReceivesDependency]
        private ProfileMenuModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            glow = CreateChild<UguiSprite>("glow", 0);
            {
                glow.Anchor = AnchorType.Fill;
                glow.RawSize = new Vector2(-60f, -60f);
                glow.SpriteName = "glow-circle-32";
                glow.Color = new Color(0f, 0f, 0f, 0.5f);
            }
            avatarDisplay = CreateChild<AvatarDisplay>("avatar", 1);
            {
                avatarDisplay.Anchor = AnchorType.Fill;
                avatarDisplay.Offset = Offset.Zero;
                avatarDisplay.MaskSprite = "circle-320";
                avatarDisplay.ImageType = Image.Type.Simple;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.CurrentUser.BindAndTrigger(OnUserChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.CurrentUser.OnNewValue -= OnUserChange;

            avatarDisplay.RemoveSource();
        }

        /// <summary>
        /// Event called when the user has changed.
        /// </summary>
        private void OnUserChange(IUser user) => avatarDisplay.SetSource(user);
    }
}