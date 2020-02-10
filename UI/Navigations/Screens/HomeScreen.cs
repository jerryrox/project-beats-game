using PBGame.UI.Components.Home;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBFramework.UI.Navigations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class HomeScreen : BaseScreen, IHomeScreen {

        public ILogoDisplay LogoDisplay { get; private set; }

        protected override int ScreenDepth => ViewDepths.HomeScreen;

        private IBackgroundOverlay BackgroundOverlay => OverlayNavigator.Get<BackgroundOverlay>();

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
                LogoDisplay.OnPress += OnLogoButton;
            }

            // Initially select a random song.
            SelectRandomMapset();

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            // Always hide menubar in home on enter.
            OverlayNavigator.Hide<MenuBarOverlay>();
        }

        /// <summary>
        /// Event called on logo button press.
        /// </summary>
        private void OnLogoButton()
        {
            LogoDisplay.SetZoom(true);
            BackgroundOverlay.Color = Color.gray;

            // Show menu bar
            OverlayNavigator.Show<MenuBarOverlay>();

            // Show home menu
            var homeMenuOverlay = OverlayNavigator.Show<HomeMenuOverlay>();
            homeMenuOverlay.OnViewHide += (isTransitioning) =>
            {
                if(!isTransitioning)
                    OverlayNavigator.Hide<MenuBarOverlay>();

                LogoDisplay.SetZoom(false);
                BackgroundOverlay.Color = Color.white;
            };
        }

        /// <summary>
        /// Selects a random mapset within the map manager.
        /// </summary>
        private void SelectRandomMapset()
        {
            // Try get a random mapset.
            var mapset = MapManager.AllMapsets.GetRandom();
            if (mapset != null)
            {
                // Select the mapset.
                MapSelection.SelectMapset(mapset);
            }
        }
    }
}