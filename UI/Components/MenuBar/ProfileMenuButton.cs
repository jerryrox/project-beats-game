using PBGame.UI.Models.MenuBar;
using PBGame.Data.Users;
using PBFramework.UI;
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
            base.OnEnableInited();

            Model.ProfileImage.OnNewValue += OnProfileImageLoaded;
            Model.CurrentUser.BindAndTrigger(OnUserChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.CurrentUser.OnNewValue -= OnUserChange;
        }

        /// <summary>
        /// Sets the profile image to the specified texture.
        /// </summary>
        private void SetProfileImage(Texture2D texture)
        {
            imageTexture.Active = texture != null;
            imageTexture.Texture = texture;
        }

        /// <summary>
        /// Sets the display for specified user profile.
        /// </summary>
        private void SetUserProfile(IUser user)
        {
            nicknameLabel.Text = user.Username;
        }

        /// <summary>
        /// Event called from cacher agent when the avatar has been loaded.
        /// </summary>
        private void OnProfileImageLoaded(Texture2D texture) => SetProfileImage(texture);

        /// <summary>
        /// Event called when the online user has changed.
        /// </summary>
        private void OnUserChange(IUser user) => SetUserProfile(user);
    }
}