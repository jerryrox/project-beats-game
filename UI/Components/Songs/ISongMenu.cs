using PBGame.UI.Components.Songs;
using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.UI.Components.Songs
{
    public interface ISongMenu : IGraphicObject
    {
        /// <summary>
        /// Returns the back button on the menu.
        /// </summary>
        IMenuButton BackButton { get; }

        /// <summary>
        /// Returns the random button on the menu.
        /// </summary>
        IMenuButton RandomButton { get; }

        /// <summary>
        /// Returns the prev button on the menu.
        /// </summary>
        IMenuButton PrevButton { get; }

        /// <summary>
        /// Returns the next button on the menu.
        /// </summary>
        IMenuButton NextButton { get; }

        /// <summary>
        /// Returns the play button on the menu.
        /// </summary>
        IMenuButton PlayButton { get; }

        /// <summary>
        /// Returns the selection preview box on the menu.
        /// </summary>
        IPreviewBox PreviewBox { get; }
    }
}