using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Offsets;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBGame.Configurations.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Overlays
{
    public class OffsetsOverlay : BaseOverlay, IOffsetsOverlay {

        private BackgroundDisplay backgroundDisplay;
        private MetronomeContainer metronomeContainer;
        private OffsetSlider mapsetSlider;
        private OffsetSlider mapSlider;

        private MapsetConfig mapsetConfig;
        private MapConfig mapConfig;


        protected override int OverlayDepth => ViewDepths.OffsetOverlay;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMapsetConfiguration MapsetConfiguration { get; set; }

        [ReceivesDependency]
        private IMapConfiguration MapConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            backgroundDisplay = CreateChild<BackgroundDisplay>("background", 0);
            {
                backgroundDisplay.Anchor = AnchorType.Fill;
                backgroundDisplay.Offset = Offset.Zero;
            }
            metronomeContainer = CreateChild<MetronomeContainer>("metronome-container", 1);
            {
                metronomeContainer.Anchor = AnchorType.BottomStretch;
                metronomeContainer.SetOffsetHorizontal(0f);
                metronomeContainer.Y = 140f;
                metronomeContainer.Height = 64f;
            }
            mapsetSlider = CreateChild<OffsetSlider>("mapset", 2);
            {
                mapsetSlider.Anchor = AnchorType.Bottom;
                mapsetSlider.Position = new Vector3(-240f, 56f);
                mapsetSlider.Size = new Vector2(400f, 100f);
                mapsetSlider.LabelText = "Mapset offset";
            }
            mapSlider = CreateChild<OffsetSlider>("map", 3);
            {
                mapSlider.Anchor = AnchorType.Bottom;
                mapSlider.Position = new Vector3(240f, 56f);
                mapSlider.Size = new Vector2(400f, 100f);
                mapSlider.LabelText = "Map offset";
            }

            OnEnableInited();
        }

        public void Setup() => Setup(MapSelection.Mapset.Value, MapSelection.Map.Value, MapSelection.MapsetConfig.Value, MapSelection.MapConfig.Value);

        public void Setup(IMapset mapset, IMap map) => Setup(mapset, map, MapsetConfiguration.GetConfig(mapset), MapConfiguration.GetConfig(map));

        protected override void OnPreHide()
        {
            base.OnPreHide();
            Dispose();
        }

        /// <summary>
        /// Internally handles setup actions.
        /// </summary>
        private void Setup(IMapset mapset, IMap map, MapsetConfig mapsetConfig, MapConfig mapConfig)
        {
            Dispose();
            
            mapsetSlider.SetSource(this.mapsetConfig = mapsetConfig);
            mapSlider.SetSource(this.mapConfig = mapConfig);
        }

        /// <summary>
        /// Disposes current mapset/map offset configuration.
        /// </summary>
        private void Dispose()
        {
            if(mapsetConfig != null)
                MapsetConfiguration.SetConfig(mapsetConfig);
            if(mapConfig != null)
                MapConfiguration.SetConfig(mapConfig);

            mapsetSlider.SetSource(mapsetConfig = null);
            mapSlider.SetSource(mapConfig = null);
        }
    }
}