using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Models.Background;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Background;
using PBGame.Maps;
using PBGame.Graphics;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class BackgroundOverlay : BaseOverlay<BackgroundModel> {

        private ParallaxContainer parallaxContainer;
        private IBackgroundDisplay emptyBackground;
        private IBackgroundDisplay imageBackground;
        private IBackgroundDisplay gradientBackground;

        private Color backgroundTint = Color.white;


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

        protected override int ViewDepth => ViewDepths.BackgroundOverlay;

        protected override bool IsRoot3D => true;

        /// <summary>
        /// Returns all backgrounds on the overlay.
        /// </summary>
        private IEnumerable<IBackgroundDisplay> Backgrounds
        {
            get
            {
                yield return emptyBackground;
                yield return imageBackground;
                yield return gradientBackground;
            }
        }


        [InitWithDependency]
        private void Init(IRoot3D root3D)
        {
            parallaxContainer = CreateChild<ParallaxContainer>("parallax", 0);
            {
                parallaxContainer.Anchor = AnchorType.Fill;
                parallaxContainer.Offset = Offset.Zero;

                emptyBackground = parallaxContainer.Content.CreateChild<EmptyBackgroundDisplay>("empty", 0);
                {
                    emptyBackground.Anchor = AnchorType.Fill;
                    emptyBackground.Offset = Offset.Zero;
                }
                imageBackground = parallaxContainer.Content.CreateChild<ImageBackgroundDisplay>("image", 1);
                {
                    imageBackground.Anchor = AnchorType.Fill;
                    imageBackground.Offset = Offset.Zero;
                    imageBackground.Active = false;
                }
                gradientBackground = parallaxContainer.Content.CreateChild<GradientBackgroundDisplay>("gradient", 2);
                {
                    gradientBackground.Anchor = AnchorType.Fill;
                    gradientBackground.Offset = Offset.Zero;
                    gradientBackground.Active = false;
                }
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            model.Background.BindAndTrigger(OnBackgroundChange);
            model.BgType.BindAndTrigger(OnBgTypeChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            model.Background.OnNewValue -= OnBackgroundChange;
            model.BgType.OnNewValue -= OnBgTypeChange;
        }

        /// <summary>
        /// Event called on map background change.
        /// </summary>
        private void OnBackgroundChange(IMapBackground background)
        {
            imageBackground.MountBackground(background);
            gradientBackground.MountBackground(background);
        }

        /// <summary>
        /// Event called on background display variant change.
        /// </summary>
        private void OnBgTypeChange(BackgroundType type)
        {
            foreach (var bg in Backgrounds)
                bg.ToggleDisplay(bg.Type == type);
        }
    }
}