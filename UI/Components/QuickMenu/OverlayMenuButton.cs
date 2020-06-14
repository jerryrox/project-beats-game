using System;
using PBFramework.UI.Navigations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.QuickMenu
{
    public class OverlayMenuButton : BaseMenuButton {

        private Type overlayType;


        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        /// <summary>
        /// Sets the overlay type to initialize with.
        /// </summary>
        public void SetOverlay<T>()
            where T : MonoBehaviour, INavigationView
        {
            overlayType = typeof(T);
            triggerAction = () => OverlayNavigator.Show<T>();
        }

        public override void OnShowQuickMenu()
        {
            IsFocused = OverlayNavigator.IsActive(overlayType);
        }
    }
}