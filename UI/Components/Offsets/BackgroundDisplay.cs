using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Navigations.Overlays;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Offsets
{
    public class BackgroundDisplay : UguiSprite {

        private BasicTrigger closeTrigger;
        

        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            SpriteName = "gradation-bottom";
            Color = Color.black;

            closeTrigger = CreateChild<BasicTrigger>("close", 0);
            {
                closeTrigger.Anchor = AnchorType.Fill;
                closeTrigger.Offset = new Offset(0f, 0f, 0f, 180f);
                
                closeTrigger.OnTriggered += () =>
                {
                    overlayNavigator.Hide<OffsetsOverlay>();
                };
            }
        }
    }
}