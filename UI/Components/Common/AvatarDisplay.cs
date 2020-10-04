using PBGame.Data.Users;
using PBGame.Graphics;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Dependencies;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common
{
    public class AvatarDisplay : UguiSprite, IHasMask {

        private WebTexture webTexture;
        private Mask mask;


        /// <summary>
        /// The sprite name of the mask.
        /// </summary>
        public string MaskSprite
        {
            get => SpriteName;
            set => SpriteName = value;
        }

        public bool UseMask
        {
            get => mask.enabled;
            set => mask.enabled = value;
        }

        public bool ShowMaskingSprite
        {
            get => mask.showMaskGraphic;
            set => mask.showMaskGraphic = value;
        }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            SpriteName = "circle-32";
            ImageType = Image.Type.Sliced;
            Color = ColorPreset.DarkBackground;
            mask = AddEffect(new MaskEffect()).Component;

            webTexture = CreateChild<WebTexture>("img");
            {
                webTexture.Anchor = AnchorType.Fill;
                webTexture.Offset = Offset.Zero;
            }
        }

        /// <summary>
        /// Sets avatar image source from specified url.
        /// </summary>
        public void SetSource(string url) => webTexture.Load(url);

        /// <summary>
        /// Sets avatar image source from specified online user.
        /// </summary>
        public void SetSource(IOnlineUser user) => webTexture.Load(user.AvatarImage);

        /// <summary>
        /// Sets avatar image source from specified user.
        /// </summary>
        public void SetSource(IUser user) => webTexture.Load(user.OnlineUser?.AvatarImage);

        /// <summary>
        /// Removes current avatar image on the texture.
        /// </summary>
        public void RemoveSource() => webTexture.Unload();
    }
}