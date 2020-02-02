using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.UI.Components.MenuBar
{
    public interface IBackgroundSprite : IGraphicObject, IHasColor {
    
        /// <summary>
        /// Returns the sprite displaying the background.
        /// </summary>
        ISprite Sprite { get; }
    }
}