using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.GameLoad
{
    public class ThumbDisplayer : UguiObject, IHasAlpha {

        private CanvasGroup canvasGroup;

        private ISprite maskSprite;
        private MapImageDisplay imageDisplay;
        private ISprite glowSprite;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            maskSprite = CreateChild<UguiSprite>("mask", 0);
            {
                maskSprite.Anchor = Anchors.Fill;
                maskSprite.Offset = Offset.Zero;
                maskSprite.SpriteName = "circle-32";
                maskSprite.ImageType = Image.Type.Sliced;
                maskSprite.Color = new Color(0f, 0f, 0f, 0.5f);

                var mask = maskSprite.AddEffect(new MaskEffect());
                mask.Component.showMaskGraphic = false;

                imageDisplay = maskSprite.CreateChild<MapImageDisplay>();
                {
                    imageDisplay.Anchor = Anchors.Fill;
                    imageDisplay.Offset = Offset.Zero;
                }
            }
            glowSprite = CreateChild<UguiSprite>("glow", 1);
            {
                glowSprite.Anchor = Anchors.Fill;
                glowSprite.Offset = new Offset(-15);
                glowSprite.SpriteName = "glow-circle-32";
                glowSprite.ImageType = Image.Type.Sliced;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            MapSelection.OnBackgroundLoaded += OnBackgroundLoad;
            OnBackgroundLoad(MapSelection.Background);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MapSelection.OnBackgroundLoaded -= OnBackgroundLoad;
            OnBackgroundLoad(MapBackground.Empty);
        }

        /// <summary>
        /// Event called on map background load.
        /// </summary>
        private void OnBackgroundLoad(IMapBackground background)
        {
            imageDisplay.SetBackground(background);
            glowSprite.Color = background.Highlight;
        }
    }
}