using System;
using PBFramework.UI;
using PBGame.UI.Components.HomeMenu;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IHomeMenuOverlay {

        /// <summary>
        /// Event called on view hide event.
        /// Returns whether the screen is transitioning to another view.
        /// </summary>
        event Action<bool> OnViewHide;


        /// <summary>
        /// Returns the background blur sprite.
        /// </summary>
        ISprite BlurSprite { get; }

        /// <summary>
        /// Returns the background gradation sprite.
        /// </summary>
        ISprite GradientSprite { get; }

        /// <summary>
        /// Returns the quit menu button.
        /// </summary>
        IMenuButton QuitButton { get; }

        /// <summary>
        /// Returns the back menu button.
        /// </summary>
        IMenuButton BackButton { get; }

        /// <summary>
        /// Returns the play menu button.
        /// </summary>
        IMenuButton PlayButton { get; }

        /// <summary>
        /// Returns the download menu button.
        /// </summary>
        IMenuButton DownloadButton { get; }
    }
}