using PBFramework.Graphics;

namespace PBGame.UI.Components.ProfileMenu
{
    public interface IStatDisplay : IGraphicObject
    {
        /// <summary>
        /// Progress displayed on the display.
        /// </summary>
        float Progress { get; set; }

        /// <summary>
        /// Text on the display label.
        /// </summary>
        string LabelText { get; set; }
    }
}