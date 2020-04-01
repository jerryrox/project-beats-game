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
        IBoxIconTrigger BackButton { get; }

        /// <summary>
        /// Returns the random button on the menu.
        /// </summary>
        IBoxIconTrigger RandomButton { get; }

        /// <summary>
        /// Returns the prev button on the menu.
        /// </summary>
        IBoxIconTrigger PrevButton { get; }

        /// <summary>
        /// Returns the next button on the menu.
        /// </summary>
        IBoxIconTrigger NextButton { get; }

        /// <summary>
        /// Returns the play button on the menu.
        /// </summary>
        IBoxIconTrigger PlayButton { get; }

        /// <summary>
        /// Returns the selection preview box on the menu.
        /// </summary>
        IPreviewBox PreviewBox { get; }
    }
}