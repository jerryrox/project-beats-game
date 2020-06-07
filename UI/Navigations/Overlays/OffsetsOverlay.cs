using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Offsets;
using PBGame.Maps;
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
            mapsetSlider = CreateChild<OffsetSlider>("mapset", 1);
            {
                mapsetSlider.LabelText = "Mapset offset";
            }
            mapSlider = CreateChild<OffsetSlider>("map", 2);
            {
                mapSlider.LabelText = "Map offset";
            }
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            var mapset = MapSelection.Mapset;
            if(mapset != null)
                mapsetSlider.SetSource(mapsetConfig = MapsetConfiguration.GetConfig(mapset));
            var map = MapSelection.Map;
            if(map != null)
                mapSlider.SetSource(mapConfig = MapConfiguration.GetConfig(map));
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();
            if(mapsetConfig != null)
                MapsetConfiguration.SetConfig(mapsetConfig);
            if(mapConfig != null)
                MapConfiguration.SetConfig(mapConfig);

            mapsetSlider.SetSource(mapsetConfig = null);
            mapSlider.SetSource(mapConfig = null);
        }
    }
}