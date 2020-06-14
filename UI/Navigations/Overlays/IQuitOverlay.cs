using System;
using PBFramework.UI.Navigations;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IQuitOverlay : INavigationView {

        /// <summary>
        /// Event called on quit animation end.
        /// </summary>
        event Action OnQuitAniEnd;
    }
}