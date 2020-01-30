using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Background;
using PBGame.Maps;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class BackgroundOverlay : BaseOverlay, IBackgroundOverlay {

        public IBackgroundDisplay ImageBackground { get; private set; }

        public IBackgroundDisplay GradientBackground { get; private set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init(IRoot3D root3D)
        {
            SetParent(root3D);

            ImageBackground = CreateChild<ImageBackgroundDisplay>("image", 0);
            {
                ImageBackground.Anchor = Anchors.Fill;
                ImageBackground.RawSize = Vector2.zero;
            }
            GradientBackground = CreateChild<GradientBackgroundDisplay>("gradient", 1);
            {
                GradientBackground.Anchor = Anchors.Fill;
                GradientBackground.RawSize = Vector2.zero;
            }
        }

        /// <summary>
        /// Mounts the specified background image to the bg displays.
        /// </summary>
        private void MountBackground(IMapBackground background)
        {
            ImageBackground.MountBackground(background);
            GradientBackground.MountBackground(background);
        }

        /// <summary>
        /// Unmounts the backgrounds from the bg displays.
        /// </summary>
        private void UnmountBackground()
        {
            ImageBackground.UnmountBackground();
            GradientBackground.UnmountBackground();
        }

        /// <summary>
        /// Binds events to external dependencies.
        /// </summary>
        private void BindEvents()
        {
            MapSelection.OnBackgroundLoaded += MountBackground;
            MapSelection.OnBackgroundUnloaded += UnmountBackground;
        }

        /// <summary>
        /// Unbinds events from external dependencies.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.OnBackgroundLoaded -= MountBackground;
            MapSelection.OnBackgroundUnloaded -= UnmountBackground;
        }

        private void OnEnable()
        {
            BindEvents();
        }

        private void OnDisable()
        {
            UnbindEvents();
        }
    }
}