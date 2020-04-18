using PBGame.Maps;
using PBFramework.Graphics;

namespace PBGame.UI.Components
{
    /// <summary>
    /// Displayer for map backgrounds.
    /// </summary>
    public interface IMapImageDisplay : IGraphicObject, IHasColor {

        /// <summary>
        /// Sets the background to display.
        /// </summary>
        void SetBackground(IMapBackground background);

        /// <summary>
        /// Dispatches FillTexture call to the current texture.
        /// </summary>
        void FillTexture();
    }
}