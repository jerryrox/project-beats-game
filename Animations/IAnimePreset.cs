using System;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Animations;

namespace PBGame.Animations
{
    /// <summary>
    /// Provides an interface for preset of anime.
    /// </summary>
    public interface IAnimePreset {

        /// <summary>
        /// Returns the default screen show animation for specified screen.
        /// </summary>
        IAnime GetDefaultScreenShow(INavigationView screen);

        /// <summary>
        /// Returns the default screen hide animation for specified screen.
        /// </summary>
        IAnime GetDefaultScreenHide(INavigationView screen);

        /// <summary>
        /// Returns the default overlay show animation for specified overlay.
        /// </summary>
        IAnime GetDefaultOverlayShow(INavigationView overlay);

        /// <summary>
        /// Returns the default overlay hide animation for specifed overlay.
        /// </summary>
        IAnime GetDefaultOverlayHide(INavigationView overlay);

        /// <summary>
        /// Returns the general submenu overlay show animation.
        /// </summary>
        IAnime GetSubMenuOverlayShow(INavigationView overlay);

        /// <summary>
        /// Returns the general submenu overlay hide animation.
        /// </summary>
        IAnime GetSubMenuOverlayHide(INavigationView overlay);

        /// <summary>
        /// Returns the general submenu overlay show & container popup animation.
        /// </summary>
        IAnime GetSubMenuOverlayPopupShow(INavigationView overlay, Func<IGraphicObject> getContainer);

        /// <summary>
        /// Returns the general submenu overlay hide & container popup animation.
        /// </summary>
        IAnime GetSubMenuOverlayPopupHide(INavigationView overlay, Func<IGraphicObject> getContainer);
    }
}