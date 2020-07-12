using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBGame.Configurations.Maps;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class OffsetsModel : BaseModel {

        private Bindable<MapsetConfig> mapsetConfig = new Bindable<MapsetConfig>();
        private Bindable<MapConfig> mapConfig = new Bindable<MapConfig>();


        /// <summary>
        /// Returns the configuration for the currently selected mapset.
        /// </summary>
        public IReadOnlyBindable<MapsetConfig> MapsetConfig => mapsetConfig;

        /// <summary>
        /// Returns the configuration for the currently selected map.
        /// </summary>
        public IReadOnlyBindable<MapConfig> MapConfig => mapConfig;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMapsetConfiguration MapsetConfiguration { get; set; }

        [ReceivesDependency]
        private IMapConfiguration MapConfiguration { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        /// <summary>
        /// Hides the offsets overlay.
        /// </summary>
        public void CloseOffsets()
        {
            OverlayNavigator.Hide<OffsetsOverlay>();
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            MapSelection.Mapset.BindAndTrigger(OnMapsetChange);
            MapSelection.Map.BindAndTrigger(OnMapChange);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            MapSelection.Mapset.OnNewValue -= OnMapsetChange;
            MapSelection.Map.OnNewValue -= OnMapChange;

            DisposeMapsetConfig();
            DisposeMapConfig();
        }

        /// <summary>
        /// Disposes currently selected mapset's configuration.
        /// </summary>
        private void DisposeMapsetConfig()
        {
            var config = mapsetConfig.Value;
            if(config != null)
                MapsetConfiguration.SetConfig(config);
        }

        /// <summary>
        /// Disposes currently selected map's configuration.
        /// </summary>
        private void DisposeMapConfig()
        {
            var config = mapConfig.Value;
            if(config != null)
                MapConfiguration.SetConfig(config);
        }

        /// <summary>
        /// Event called when the selected mapset has changed.
        /// </summary>
        private void OnMapsetChange(IMapset mapset)
        {
            DisposeMapsetConfig();
            mapsetConfig.Value = MapsetConfiguration.GetConfig(mapset);
        }

        /// <summary>
        /// Event called when the selected map has changed.
        /// </summary>
        private void OnMapChange(IMap map)
        {
            DisposeMapConfig();
            mapConfig.Value = MapConfiguration.GetConfig(map);
        }
    }
}