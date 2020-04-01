using PBFramework.UI;

namespace PBGame.UI.Components.HomeMenu
{
    public interface IMenuButton : IButtonTrigger {
    
        /// <summary>
        /// Returns the icon sprite.
        /// </summary>
        ISprite IconSprite { get; }

        /// <summary>
        /// Returns the flash icon sprite.
        /// </summary>
        ISprite FlashSprite { get; }

        /// <summary>
        /// Returns the label on the menu button.
        /// </summary>
        ILabel Label { get; }
    }
}