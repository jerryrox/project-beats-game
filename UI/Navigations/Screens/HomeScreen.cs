using PBGame.UI.Models;
using PBGame.UI.Components.Home;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBFramework.UI.Navigations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class HomeScreen : BaseScreen<HomeModel> {

        private LogoDisplay logoDisplay;


        protected override int ViewDepth => ViewDepths.HomeScreen;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            logoDisplay = CreateChild<LogoDisplay>("logo", 10);
            {
                logoDisplay.Size = new Vector2(352f, 352f);
                logoDisplay.OnPress += model.ShowHomeMenuOverlay;
            }

            model.IsHomeMenuShown.OnNewValue += OnHomeMenuToggle;
        }

        /// <summary>
        /// Event called from model when the home menu overlay's toggle state has changed.
        /// </summary>
        private void OnHomeMenuToggle(bool isShown)
        {
            logoDisplay.SetZoom(isShown);
        }
    }
}