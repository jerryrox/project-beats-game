using System;
using System.Collections.Generic;
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
            EmptyBackground = CreateChild<EmptyBackgroundDisplay>("empty", 0);
            {
                EmptyBackground.Anchor = Anchors.Fill;
                EmptyBackground.RawSize = Vector2.zero;
            }
            ImageBackground = CreateChild<ImageBackgroundDisplay>("image", 1);
            {
                ImageBackground.Anchor = Anchors.Fill;
                ImageBackground.RawSize = Vector2.zero;
                ImageBackground.Active = false;
            }
            GradientBackground = CreateChild<GradientBackgroundDisplay>("gradient", 2);
            {
                GradientBackground.Anchor = Anchors.Fill;
                GradientBackground.RawSize = Vector2.zero;
                GradientBackground.Active = false;
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
        private void ChangeBgDisplay(INavigationView screen)
        {
            // TODO: Display different background if required.
            SetBackground(ImageBackground);
        }

        /// <summary>
        /// Binds events to external dependencies.
        /// </summary>
        private void BindEvents()
        {
            MapSelection.OnBackgroundLoaded += MountBackground;
            MountBackground(MapSelection.Background);

            ScreenNavigator.OnShowView += ChangeBgDisplay;
            ChangeBgDisplay(ScreenNavigator.CurrentScreen);
        }

        /// <summary>
        /// Unbinds events from external dependencies.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.OnBackgroundLoaded -= MountBackground;
            ScreenNavigator.OnShowView -= ChangeBgDisplay;
        }
    }
}