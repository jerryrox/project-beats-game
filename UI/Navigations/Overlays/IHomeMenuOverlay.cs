using System;
using PBFramework.UI;
using PBGame.UI.Components.HomeMenu;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IHomeMenuOverlay {

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
        MenuButton QuitButton { get; }

        /// <summary>
        /// Returns the back menu button.
        /// </summary>
        MenuButton BackButton { get; }

        /// <summary>
        /// Returns the play menu button.
        /// </summary>
        MenuButton PlayButton { get; }

        /// <summary>
        /// Returns the download menu button.
        /// </summary>
        MenuButton DownloadButton { get; }
    }
}