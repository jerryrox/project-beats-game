using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.System;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class SystemOverlay : BaseOverlay, ISystemOverlay {


        public FpsDisplayer FpsDisplayer { get; private set; }

        protected override int OverlayDepth => ViewDepths.SystemOverlay;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            FpsDisplayer = CreateChild<FpsDisplayer>("fps-displayer", 0);
            {
                FpsDisplayer.Anchor = Anchors.BottomRight;
                FpsDisplayer.Pivot = Pivots.BottomRight;
                FpsDisplayer.Position = new Vector3(-12f, 12);
                FpsDisplayer.Size = new Vector2(170f, 30f);

                FpsDisplayer.Active = GameConfiguration.ShowFps.Value;
            }
        }
    }
}