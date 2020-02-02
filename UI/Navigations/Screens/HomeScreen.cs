using PBGame.Maps;
using PBGame.UI.Components.Home;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class HomeScreen : BaseScreen, IHomeScreen {

        public ILogoDisplay LogoDisplay { get; private set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }


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
        }

        /// <summary>
        /// Event called on logo button press.
        /// </summary>
        private void OnLogoButton()
        {
            // TODO: Spawn menu bar overlay.

            // TODO: Spawn home menu overlay.
        }

        /// <summary>
        /// Selects a random mapset within the map manager.
        /// </summary>
        private void SelectRandomMapset()
        {
            // Try get a random mapset.
            var mapset = MapManager.DisplayedMapsets.GetRandom();
            if (mapset != null)
            {
                // Select the mapset.
                MapSelection.SelectMapset(mapset);
            }
        }
    }
}