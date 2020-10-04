using PBGame.UI.Models.MenuBar;
using PBGame.UI.Components.Common;
using PBGame.Data.Users;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MenuBar
{
    public class ProfileMenuButton : BaseMenuButton {

        private ISprite background;
        private AvatarDisplay avatarDisplay;
        private ILabel nicknameLabel;
        private ILabel levelLabel;


        protected override MenuType Type => MenuType.Profile;

        protected override string IconSpritename => "";

        protected override bool OverrideEnableInitCall => true;


        [InitWithDependency]
        private void Init()
        {
            background = CreateChild<UguiSprite>("background", -1);
            {
                background.Anchor = AnchorType.Fill;
                background.RawSize = Vector2.zero;
                background.Color = new Color(0f, 0f, 0f, 0.125f);
            }
            avatarDisplay = CreateChild<AvatarDisplay>("avatar", 5);
            {
                avatarDisplay.Anchor = AnchorType.Left;
                avatarDisplay.Pivot = PivotType.Left;
                avatarDisplay.X = 8f;
                avatarDisplay.Size = new Vector2(48f, 48f);
                avatarDisplay.Color = new Color(0f, 0f, 0f, 0.125f);
                avatarDisplay.MaskSprite = null;
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
        /// Sets the display for specified user profile.
        /// </summary>
        private void SetUserProfile(IUser user)
        {
            nicknameLabel.Text = user.Username;
            avatarDisplay.SetSource(user);
        }

        /// <summary>
        /// Event called when the online user has changed.
        /// </summary>
        private void OnUserChange(IUser user) => SetUserProfile(user);
    }
}