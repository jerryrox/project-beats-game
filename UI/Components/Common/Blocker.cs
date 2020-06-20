using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// An object which simply blocks user input by intercepting raycasts.
    /// </summary>
    public class Blocker : UguiSprite {

        [InitWithDependency]
        private void Init()
        {
            Anchor = AnchorType.Fill;
            Offset = Offset.Zero;
            SpriteName = "null";
        }
    }
}