using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// An object which simply blocks user input by intercepting raycasts.
    /// </summary>
    public class Blocker : BasicTrigger {

        private ISprite backgroundSprite;


        /// <summary>
        /// Returns the background sprite blocking the raycast.
        /// </summary>
        public ISprite Background => backgroundSprite;

        protected override string TriggerAudio => null;

        protected override string PointerEnterAudio => null;


        [InitWithDependency]
        private void Init()
        {
            Anchor = AnchorType.Fill;
            Offset = Offset.Zero;
            
            backgroundSprite = CreateChild<UguiSprite>("bg", -1);
            {
                backgroundSprite.Anchor = AnchorType.Fill;
                backgroundSprite.Offset = Offset.Zero;
                backgroundSprite.SpriteName = "null";
            }
        }
    }
}