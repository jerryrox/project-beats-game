using PBGame.UI.Components.System;
using PBFramework.UI.Navigations;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public interface ISystemOverlay : INavigationView {
        
        /// <summary>
        /// Returns the fps displayer component.
        /// </summary>
        FpsDisplayer FpsDisplayer { get; }

        /// <summary>
        /// Returns the message displayer component.
        /// </summary>
        MessageDisplayer MessageDisplayer { get; }
    }
}