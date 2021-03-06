using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Data.Users;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class CoverDisplay : UguiObject
    {
        private WebTexture image;


        [ReceivesDependency]
        private ProfileMenuModel Model { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            var bg = CreateChild<UguiSprite>("bg");
            {
                bg.Anchor = AnchorType.Fill;
                bg.Offset = Offset.Zero;
                bg.Color = ColorPreset.DarkBackground;
            }
            image = CreateChild<WebTexture>("image");
            {
                image.Anchor = AnchorType.Fill;
                image.Offset = Offset.Zero;
            }
            var shadow = CreateChild<UguiSprite>("shadow");
            {
                shadow.Anchor = AnchorType.BottomStretch;
                shadow.Pivot = PivotType.Bottom;
                shadow.SetOffsetHorizontal(0f);
                shadow.Y = 0f;
                shadow.Height = 32f;
                shadow.Color = new Color(0f, 0f, 0f, 0.5f);
                shadow.SpriteName = "gradation-bottom";
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
            
            Model.CurrentUser.Unbind(OnUserChange);

            image.Unload();
        }

        /// <summary>
        /// Event called when the current user has changed.
        /// </summary>
        private void OnUserChange(IUser user)
        {
            if (user.IsOnlineUser)
                image.Load(user.OnlineUser.CoverImage);
        }
    }
}