using PBGame.UI.Models.Background;
using PBGame.Maps;
using PBFramework.Graphics;

namespace PBGame.UI.Components.Background
{
    public interface IBackgroundDisplay : IGraphicObject, IHasColor, IHasAlpha {

        /// <summary>
        /// Returns the type of background variant represented.
        /// </summary>
        BackgroundType Type { get; }


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