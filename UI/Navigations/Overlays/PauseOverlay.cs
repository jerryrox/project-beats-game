using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data;
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

        protected override bool IsDerived => true;

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            SetMessage("Game paused");

            AddSelection("Resume", ColorPreset.Positive, () =>
            {
                OverlayNavigator.Hide(this);
                GameSession?.InvokeResume();
            });
            AddSelection("Offset", ColorPreset.Passive, () =>
            {
                // Show the offset overlay.
                OffsetsOverlay overlay = OverlayNavigator.Show<OffsetsOverlay>();
                overlay.Setup();

                // Bind temporary hide event listener to overlay.
                EventBinder<Action> closeBinder = new EventBinder<Action>(
                    e => overlay.OnHide += e,
                    e => overlay.OnHide -= e
                );
                closeBinder.IsOneTime = true;
                // Show pause overlay once the offset overlay has been closed.
                IGameSession savedSession = GameSession;
                closeBinder.SetHandler(() =>
                {
                    var pause = OverlayNavigator.Show<PauseOverlay>();
                    pause.GameSession = savedSession;
                });
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