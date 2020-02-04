using PBGame.Maps;
using PBFramework.Graphics;

namespace PBGame.UI.Components.Background
{
    public interface IBackgroundDisplay : IGraphicObject, IHasColor, IHasAlpha {

        /// <summary>
        /// Mounts the specified background to this display.
        /// </summary>
        void MountBackground(IMapBackground background);

        /// <summary>
        /// Sets whether the display should be displayed.
        /// </summary>
        void ToggleDisplay(bool enable);
    }
}