using System;

namespace PBGame.UI.Navigations.Overlays
{
    public interface ISubMenuOverlay {

        /// <summary>
        /// Event called when the submenu has been closed within its context.
        /// Reset when hidden.
        /// </summary>
        event Action OnClose;
    }
}