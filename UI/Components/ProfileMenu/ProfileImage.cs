using PBGame.UI.Models;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class ProfileImage : UguiObject {

        private ISprite glow;
        private ISprite mask;
        private ITexture image;


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
            mask = CreateChild<UguiSprite>("mask", 1);
            {
                mask.Anchor = AnchorType.Fill;
                mask.RawSize = Vector2.zero;
                mask.Color = new Color(0.125f, 0.125f, 0.125f);
                mask.SpriteName = "circle-320";

                mask.AddEffect(new MaskEffect());

                image = mask.CreateChild<UguiTexture>("image", 2);
                {
                    image.Anchor = AnchorType.Fill;
                    image.RawSize = Vector2.zero;
                }
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.ProfileImage.BindAndTrigger(OnProfileImageChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.ProfileImage.OnNewValue -= OnProfileImageChange;

            image.Active = false;
        }

        /// <summary>
        /// Event called when the user's profile image is changed.
        /// </summary>
        private void OnProfileImageChange(Texture2D profileImage)
        {
            image.Texture = profileImage;
            image.Active = profileImage != null;
        }
    }
}