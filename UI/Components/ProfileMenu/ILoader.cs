using PBFramework.Graphics;

namespace PBGame.UI.Components.ProfileMenu
{
    public interface ILoader : IGraphicObject, IHasAlpha {

        /// <summary>
        /// Shows the loader display.
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the loader display.
        /// </summary>
        void Hide();
    }
}