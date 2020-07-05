using PBGame.UI.Models;
using PBGame.UI.Components.Home;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBFramework.UI.Navigations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class HomeScreen : BaseScreen<HomeModel>, IHomeScreen {


        public LogoDisplay LogoDisplay { get; private set; }

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
            LogoDisplay = CreateChild<LogoDisplay>("logo", 10);
            {
                LogoDisplay.Size = new Vector2(352f, 352f);
                LogoDisplay.OnPress += model.ShowHomeMenuOverlay;
            }

            model.IsHomeMenuShown.OnNewValue += OnHomeMenuToggle;
        }

        protected override HomeModel CreateModel() => new HomeModel();

        /// <summary>
        /// Event called from model when the home menu overlay's toggle state has changed.
        /// </summary>
        private void OnHomeMenuToggle(bool isShown)
        {
            LogoDisplay.SetZoom(isShown);
        }
    }
}