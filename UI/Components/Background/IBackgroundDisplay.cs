using PBGame.Maps;
using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.UI.Components.Background
{
    public interface IBackgroundDisplay : IGraphicObject, IHasColor, IHasAlpha {

        /// <summary>
        /// Mounts the specified background to this display.
        /// </summary>
        void MountBackground(IMapBackground background);

        /// <summary>
        /// Unmounts the current background from this display.
        /// </summary>
        void UnmountBackground();
    }
}