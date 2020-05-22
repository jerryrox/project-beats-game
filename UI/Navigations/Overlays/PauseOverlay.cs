using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Overlays
{
    public class PauseOverlay : DialogOverlay, IPauseOverlay
    {
        /// <summary>
        /// The game session which triggered this overlay.
        /// </summary>
        public IGameSession GameSession { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            AddSelection("Resume", ColorPreset.Positive, () =>
            {
                OverlayNavigator.Hide(this);
                GameSession?.InvokeResume();
            });
            AddSelection("Offset", ColorPreset.Passive, () =>
            {
                // TODO: Show offset settings overlay.
            });
            AddSelection("Retry", ColorPreset.Warning, () =>
            {
                GameSession?.InvokeRetry();
            });
            AddSelection("Quit", ColorPreset.Negative, () =>
            {
                GameSession?.InvokeForceQuit();
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameSession = null;
        }
    }
}