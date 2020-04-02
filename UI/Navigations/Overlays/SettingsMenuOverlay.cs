using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.UI.Navigations.Overlays
{
    public class SettingsMenuOverlay : BaseSubMenuOverlay, ISettingsMenuOverlay {

        protected override int OverlayDepth => ViewDepths.SettingsMenuOverlay;

        public SettingsMenuOverlay()
        {
            
        }
    }
}