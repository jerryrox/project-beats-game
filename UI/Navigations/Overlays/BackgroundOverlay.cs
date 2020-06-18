using System;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Background;
using PBGame.UI.Navigations.Screens;
using PBGame.Maps;
using PBGame.Graphics;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class BackgroundOverlay : BaseOverlay, IBackgroundOverlay {

        private ParallaxContainer parallaxContainer;

        private Color backgroundTint = Color.white;


        public IBackgroundDisplay EmptyBackground { get; private set; }

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

        protected override int OverlayDepth => ViewDepths.BackgroundOverlay;

        protected override bool IsRoot3D => true;

        /// <summary>
        /// Returns all backgrounds on the overlay.
        /// </summary>
        private IEnumerable<IBackgroundDisplay> Backgrounds
        {
            get
            {
                yield return EmptyBackground;
                yield return ImageBackground;
                yield return GradientBackground;
            }
        }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        [InitWithDependency]
        private void Init(IRoot3D root3D)
        {
            parallaxContainer = CreateChild<ParallaxContainer>("parallax", 0);
            {
                parallaxContainer.Anchor = AnchorType.Fill;
                parallaxContainer.Offset = Offset.Zero;

                EmptyBackground = parallaxContainer.Content.CreateChild<EmptyBackgroundDisplay>("empty", 0);
                {
                    EmptyBackground.Anchor = AnchorType.Fill;
                    EmptyBackground.Offset = Offset.Zero;
                }
                ImageBackground = parallaxContainer.Content.CreateChild<ImageBackgroundDisplay>("image", 1);
                {
                    ImageBackground.Anchor = AnchorType.Fill;
                    ImageBackground.Offset = Offset.Zero;
                    ImageBackground.Active = false;
                }
                GradientBackground = parallaxContainer.Content.CreateChild<GradientBackgroundDisplay>("gradient", 2);
                {
                    GradientBackground.Anchor = AnchorType.Fill;
                    GradientBackground.Offset = Offset.Zero;
                    GradientBackground.Active = false;
                }
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            BindEvents();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnbindEvents();
        }

        public void SetBackground(IBackgroundDisplay display)
        {
            foreach (var bg in Backgrounds)
                bg.ToggleDisplay(bg == display);
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
        /// Adjusts background display matching the specified screen.
        /// </summary>
        private void OnScreenChange(INavigationView screen)
        {
            // TODO: Display different background if required.
            SetBackground(ImageBackground);
        }

        /// <summary>
        /// Binds events to external dependencies.
        /// </summary>
        private void BindEvents()
        {
            MapSelection.Background.BindAndTrigger(MountBackground);

            ScreenNavigator.CurrentScreen.BindAndTrigger(OnScreenChange);
        }

        /// <summary>
        /// Unbinds events from external dependencies.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.Background.OnNewValue -= MountBackground;
            ScreenNavigator.CurrentScreen.OnNewValue -= OnScreenChange;
        }
    }
}