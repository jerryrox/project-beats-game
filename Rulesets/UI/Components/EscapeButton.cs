using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Utils;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.UI.Components
{
    public class EscapeButton : UguiObject {

        private BasicTrigger buttonTrigger;

        private UguiObject indicatorContainer;
        private CanvasGroup indicatorCanvas;
        private UguiSprite bgSprite;
        private FlipEffect bgFlipEffect;
        private UguiSprite shadowSprite;
        private FlipEffect shadowFlipEffect;
        private UguiSprite iconSprite;

        private IAnime showAni;
        private IAnime hideAni;

        private BindableBool isTriggered = new BindableBool(false);


        /// <summary>
        /// Returns whether the button is triggered on.
        /// </summary>
        public IReadOnlyBindable<bool> IsTriggered => isTriggered;

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Width = 0f;

            isTriggered.Bind(OnTriggeredChange);

            buttonTrigger = CreateChild<BasicTrigger>("trigger");
            {
                buttonTrigger.Anchor = AnchorType.CenterStretch;
                buttonTrigger.Width = 12f;
                buttonTrigger.SetOffsetVertical(0f);

                buttonTrigger.OnPointerDown += () => isTriggered.Value = true;
                buttonTrigger.OnPointerUp += () => isTriggered.Value = false;

                var bg = buttonTrigger.CreateChild<UguiSprite>("bg");
                {
                    bg.Anchor = AnchorType.CenterStretch;
                    bg.SetOffsetVertical(0f);
                    bg.Width = 8f;
                    bg.Color = new Color(1f, 1f, 1f, 0.25f);
                    bg.SpriteName = "circle-8";
                    bg.ImageType = Image.Type.Sliced;
                }
            }
            indicatorContainer = CreateChild<UguiObject>("container");
            {
                indicatorContainer.Anchor = AnchorType.CenterStretch;
                indicatorContainer.Width = 0;
                indicatorContainer.SetOffsetVertical(0f);

                indicatorCanvas = indicatorContainer.RawObject.AddComponent<CanvasGroup>();
                indicatorCanvas.alpha = 0f;

                shadowSprite = indicatorContainer.CreateChild<UguiSprite>("shadow");
                {
                    shadowSprite.Anchor = AnchorType.Fill;
                    shadowSprite.Offset = new Offset(0f, 9f, 0f, -9f);
                    shadowSprite.Color = ColorPreset.Passive.Darken(0.5f);
                    shadowSprite.SpriteName = "parallel-64";
                    shadowSprite.ImageType = Image.Type.Sliced;

                    shadowFlipEffect = shadowSprite.AddEffect(new FlipEffect());
                }
                bgSprite = indicatorContainer.CreateChild<UguiSprite>("bg");
                {
                    bgSprite.Anchor = AnchorType.Fill;
                    bgSprite.Offset = Offset.Zero;
                    bgSprite.Color = ColorPreset.Passive;
                    bgSprite.SpriteName = "parallel-64";
                    bgSprite.ImageType = Image.Type.Sliced;

                    bgFlipEffect = bgSprite.AddEffect(new FlipEffect());
                }
                iconSprite = indicatorContainer.CreateChild<UguiSprite>("icon");
                {
                    iconSprite.Y = 0f;
                    iconSprite.Size = new Vector2(24f, 24f);
                    iconSprite.SpriteName = "icon-pause";
                    iconSprite.Color = ColorPreset.PrimaryFocus;
                }
            }

            showAni = new Anime();
            showAni.AnimateFloat((width) => indicatorContainer.Width = width)
                .AddTime(0f, 0, EaseType.BackEaseOut)
                .AddTime(0.25f, 196f)
                .Build();
            showAni.AnimateFloat((alpha) => indicatorCanvas.alpha = alpha)
                .AddTime(0f, () => indicatorCanvas.alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat((width) => indicatorContainer.Width = width)
                .AddTime(0f, 196f, EaseType.CubicEaseIn)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AnimateFloat((alpha) => indicatorCanvas.alpha = alpha)
                .AddTime(0f, () => indicatorCanvas.alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        /// <summary>
        /// Sets the component display pivot to the specified type.
        /// The only supported types are either left or right.
        /// </summary>
        public void SetSide(PivotType side)
        {
            if(side != PivotType.Left && side != PivotType.Right)
                return;

            bool isLeftSide = side == PivotType.Left;
            float forwardDir = isLeftSide ? 1f : -1f;
            AnchorType stretchedAnchor = isLeftSide ? AnchorType.LeftStretch : AnchorType.RightStretch;
            AnchorType oppositeAnchor = isLeftSide ? AnchorType.Right : AnchorType.Left;

            this.Pivot = side;

            buttonTrigger.X = 4f * forwardDir;

            indicatorContainer.Anchor = stretchedAnchor;
            indicatorContainer.Pivot = side;
            indicatorContainer.X = -32f * forwardDir;

            bgFlipEffect.Component.horizontal = !isLeftSide;
            shadowFlipEffect.Component.horizontal = !isLeftSide;
            shadowSprite.SetOffsetHorizontal(-4, -4);

            iconSprite.Anchor = oppositeAnchor;
            iconSprite.X = -64 * forwardDir;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            isTriggered.Value = false;
        }

        /// <summary>
        /// Event called when the triggered state is changed.
        /// </summary>
        private void OnTriggeredChange(bool isTriggered)
        {
            if (isTriggered)
            {
                hideAni.Stop();
                showAni.PlayFromStart();
            }
            else
            {
                showAni.Stop();
                hideAni.PlayFromStart();
            }
        }
    }
}