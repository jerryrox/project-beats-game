using PBFramework.UI.Navigations;
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
        IAnime GetDefaultOverlayHIde(INavigationView overlay);

        /// <summary>
        /// Returns the default animation for initialize logo startup.
        /// </summary>
        IAnime GetInitLogoStartup(UI.Components.Initialize.ILogoDisplay logoDisplay);

        /// <summary>
        /// Returns the default animation for initialize logo breathe.
        /// </summary>
        IAnime GetInitLogoBreathe(UI.Components.Initialize.ILogoDisplay logoDisplay);

        /// <summary>
        /// Returns the default animation for initialize logo end.
        /// </summary>
        IAnime GetInitLogoEnd(UI.Components.Initialize.ILogoDisplay logoDisplay);

        /// <summary>
        /// Returns the home screen logo pulsating animation.
        /// </summary>
        IAnime GetHomeLogoPulse(UI.Components.Home.ILogoDisplay logoDisplay);

        /// <summary>
        /// Returns the home screen logo on-hover animation.
        /// </summary>
        IAnime GetHomeLogoHover(UI.Components.Home.ILogoDisplay logoDisplay);

        /// <summary>
        /// Returns the home screen logo on-exit animation.
        /// </summary>
        IAnime GetHomeLogoExit(UI.Components.Home.ILogoDisplay logoDisplay);
    }
}