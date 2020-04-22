using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.System
{
    public class FpsDisplayer : UguiObject, IHasTint {

        /// <summary>
        /// Number of frames to sample for calculating FPS.
        /// </summary>
        private const int SampleCount = 15;
        private const float SampleReciprocal = 1f / SampleCount;

        /// <summary>
        /// Fps threshold which triggers warning performance state.
        /// </summary>
        private const float WarningThresholdFps = 50f;

        /// <summary>
        /// Fps threshold which triggers bad performance state.
        /// </summary>
        private const float BadThresholdFps = 40f;

        private CanvasGroup canvasGroup;

        private ISprite shadowSprite;
        private ISprite backgroundSprite;
        private ISprite separatorSprite;
        private ILabel fpsLabel;
        private ILabel timeLabel;

        private Color tint;

        private float sampleSum = 0f;
        private int curSamples = 0;
        private FpsStateType lastFpsState;

        private IAnime showAni;
        private IAnime hideAni;


        public Color Tint
        {
            get => tint;
            set
            {
                shadowSprite.Tint = value;
                backgroundSprite.Tint = value;
            }
        }

        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        /// <summary>
        /// Returns the fps state last observed.
        /// </summary>
        public FpsStateType FpsState => lastFpsState;

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = RawObject.AddComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            tint = ColorPreset.Positive;

            shadowSprite = CreateChild<UguiSprite>("shadow", 0);
            {
                shadowSprite.Anchor = Anchors.Fill;
                shadowSprite.Offset = new Offset(-13.5f);
                shadowSprite.Color = tint;
                shadowSprite.Alpha = 0.3f;
                shadowSprite.SpriteName = "glow-circle-16-x2";
                shadowSprite.ImageType = Image.Type.Sliced;
            }
            backgroundSprite = CreateChild<UguiSprite>("bg", 1);
            {
                backgroundSprite.Anchor = Anchors.Fill;
                backgroundSprite.Offset = Offset.Zero;
                backgroundSprite.Color = tint;
                backgroundSprite.Alpha = 0.5f;
                backgroundSprite.SpriteName = "circle-16";
                backgroundSprite.ImageType = Image.Type.Sliced;
            }
            separatorSprite = CreateChild<UguiSprite>("separator", 2);
            {
                separatorSprite.Anchor = Anchors.CenterStretch;
                separatorSprite.SetOffsetVertical(0f);
                separatorSprite.X = 0f;
                separatorSprite.Width = 2f;
                separatorSprite.Color = new Color(0f, 0f, 0f, 0.5f);
            }
            fpsLabel = CreateChild<Label>("fps", 3);
            {
                fpsLabel.FontSize = 16;
                fpsLabel.WrapText = true;
                fpsLabel.RawTransform.anchorMin = Vector2.zero;
                fpsLabel.RawTransform.anchorMax = new Vector2(0.5f, 1f);
                fpsLabel.Color = Color.black;
            }
            timeLabel = CreateChild<Label>("time", 4);
            {
                timeLabel.FontSize = 16;
                timeLabel.WrapText = true;
                timeLabel.RawTransform.anchorMin = new Vector2(0.5f, 0f);
                timeLabel.RawTransform.anchorMax = Vector2.one;
                timeLabel.Color = Color.black;
            }

            showAni = new Anime();
            showAni.AddEvent(0f, () => Active = true);
            showAni.AnimateFloat(a => Alpha = a)
                .AddTime(0f, () => Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat(a => Alpha = a)
                .AddTime(0f, () => Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AddEvent(0.25f, () => Active = false);

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            sampleSum = 0f;
            curSamples = 0;
            SetFpsState(FpsStateType.Good, true);
            Refresh();
        }

        /// <summary>
        /// Toggles displayer active state.
        /// </summary>
        public void ToggleDisplay(bool enable)
        {
            if (enable && !Active)
                showAni.PlayFromStart();
            else if(!enable && Active)
                hideAni.PlayFromStart();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            if (curSamples < SampleCount)
            {
                sampleSum += deltaTime;
                curSamples++;
            }
            else
            {
                sampleSum = sampleSum - (sampleSum * SampleReciprocal) + deltaTime;
            }
            Refresh();
        }

        /// <summary>
        /// Refreshes the display labels and tint.
        /// </summary>
        private void Refresh()
        {
            if (curSamples < SampleCount)
            {
                fpsLabel.Text = "0 fps";
                timeLabel.Text = "0 ms";
                return;
            }

            float averageDelta = sampleSum * 1000f * SampleReciprocal;
            float averageFps = 1000f / averageDelta;

            fpsLabel.Text = $"{averageFps.ToString("F1")} fps";
            timeLabel.Text = $"{averageDelta.ToString("F1")} ms";

            if(averageFps < BadThresholdFps)
                SetFpsState(FpsStateType.Bad, false);
            else if (averageFps < WarningThresholdFps)
                SetFpsState(FpsStateType.Warning, false);
            else
                SetFpsState(FpsStateType.Good, false);
        }

        /// <summary>
        /// Sets the current fps state type to specified value and visually reflects it using tint.
        /// </summary>
        private void SetFpsState(FpsStateType type, bool force)
        {
            if(!force && type == lastFpsState)
                return;

            lastFpsState = type;
            switch (type)
            {
                case FpsStateType.Good:
                    Tint = ColorPreset.Positive;
                    break;
                case FpsStateType.Warning:
                    Tint = ColorPreset.Warning;
                    break;
                case FpsStateType.Bad:
                    Tint = ColorPreset.Negative;
                    break;
                default:
                    throw new Exception("Unsupported FpsStateType: " + type);
            }
        }
    }
}