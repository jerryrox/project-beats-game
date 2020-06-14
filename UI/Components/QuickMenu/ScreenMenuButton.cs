using System;
using PBFramework.UI.Navigations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.QuickMenu
{
    public class ScreenMenuButton : BaseMenuButton {

        private Type screenType;


        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        /// <summary>
        /// Sets the screen type to initialize with.
        /// </summary>
        public void SetScreen<T>()
            where T : MonoBehaviour, INavigationView
        {
            screenType = typeof(T);
            triggerAction = () => ScreenNavigator.Show<T>();
        }

        public override void OnShowQuickMenu()
        {
            IsFocused = ScreenNavigator.IsActive(screenType);
        }
    }
}