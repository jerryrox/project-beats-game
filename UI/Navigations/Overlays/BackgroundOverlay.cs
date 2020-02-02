using System.Collections.Generic;
using PBGame.UI.Components.Background;
using PBGame.Maps;
using PBGame.Graphics;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class BackgroundOverlay : BaseOverlay, IBackgroundOverlay {

        private Color backgroundTint = Color.white;


        public IBackgroundDisplay ImageBackground { get; private set; }

        public IBackgroundDisplay GradientBackground { get; private set; }

        public Color Color
        {
            get => backgroundTint;
            set
            {
                backgroundTint = value;
                foreach(var background in Backgrounds)
                    background.Color = value;
            }
        }

        /// <summary>
        /// Returns all backgrounds on the overlay.
        /// </summary>
        private IEnumerable<IBackgroundDisplay> Backgrounds
        {
            get
            {
                yield return ImageBackground;
                yield return GradientBackground;
            }
        }

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

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            BindEvents();
        }

        protected override void OnDisable()
        {
            UnbindEvents();
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
            
            MountBackground(MapSelection.Background);
        }

        /// <summary>
        /// Unbinds events from external dependencies.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.OnBackgroundLoaded -= MountBackground;
            MapSelection.OnBackgroundUnloaded -= UnmountBackground;
        }
    }
}