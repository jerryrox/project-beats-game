using PBGame.UI.Models;
using PBGame.Data.Users;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class Header : UguiObject {

        private ProfileImage profileImage;
        private ILabel nickname;


        [ReceivesDependency]
        private ProfileMenuModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            profileImage = CreateChild<ProfileImage>("profile", 0);
            {
                profileImage.Anchor = AnchorType.Bottom;
                profileImage.Size = new Vector2(100f, 100f);
                profileImage.Y = 94f;
            }
            nickname = CreateChild<Label>("nickname", 1);
            {
                nickname.Anchor = AnchorType.BottomStretch;
                nickname.RawWidth = -64f;
                nickname.Y = 24f;
                nickname.Height = 30;
                nickname.Alignment = TextAnchor.MiddleCenter;
                nickname.IsBold = true;
                nickname.WrapText = true;
                nickname.FontSize = 18;
            }
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
        }

        /// <summary>
        /// Event called when the current user has changed.
        /// </summary>
        private void OnUserChange(IUser user)
        {
            nickname.Text = user.Username;
        }
    }
}