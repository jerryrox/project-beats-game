using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.UI.Components
{
    public class SkipDisplay : UguiSprite, IHasAlpha {

        /// <summary>
        /// Minimum amount of time required before first object's start time, to allow skipping.
        /// </summary>
        private const float MinSkipOffset = (60000f / 320f) * SkipBeatCount; // 320 BPM

        /// <summary>
        /// Maximum skip offset clamp value.
        /// </summary>
        private const float MaxSkipOffset = (60000f / 120f) * SkipBeatCount; // 120 BPM

        /// <summary>
        /// Number of beats ahead of first object' start time for skipToTime calculation.
        /// </summary>
        private const int SkipBeatCount = 8;

        private CanvasGroup canvasGroup;

        private DialogButton skipButton;
        private ILabel timeLabel;

        private IAnime showAni;
        private IAnime hideAni;

        private float skipToTime;


        public new float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        /// <summary>
        /// Returns whether the show/hide animations are playing.
        /// </summary>
        private bool IsAnimating => showAni.IsPlaying || hideAni.IsPlaying;

        [ReceivesDependency]
        private IGameSession GameSession { get; set; }
        
        [ReceivesDependency]
        private IMusicController MusicController { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            GameSession.OnHardInit += () =>
            {
                OnHardInit();
            };
            GameSession.OnSoftInit += () =>
            {
                OnSoftInit();
            };
            GameSession.OnSoftDispose += () =>
            {
                OnSoftDispose();
            };

            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            SpriteName = "gradation-bottom";
            Offset = Offset.Zero;
            Color = Color.black;

            skipButton = CreateChild<DialogButton>("button", 0);
            {
                skipButton.Anchor = AnchorType.Bottom;
                skipButton.Position = new Vector3(0f, 50f);
                skipButton.Label.Y = 10f;
                skipButton.Tint = colorPreset.Passive;
                skipButton.LabelText = "Skip";
                skipButton.OnTriggered += OnSkipButton;

                timeLabel = skipButton.CreateChild<Label>("time", 10);
                {
                    timeLabel.FontSize = 17;
                    timeLabel.Alignment = TextAnchor.MiddleCenter;
                    timeLabel.Y = -10f;
                }
            }

            showAni = new Anime();
            showAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AddEvent(0f, showAni.Stop);
            hideAni.AddEvent(hideAni.Duration, () => gameObject.SetActive(false));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (showAni != null)
                showAni.Stop();
            if(hideAni != null)
                hideAni.Stop();
        }

        protected void Update()
        {
            float remainingTime = skipToTime - GameSession.GameProcessor.CurrentTime;

            // Automatically hide skip display if over skip time.
            if (remainingTime <= 0f && !IsAnimating)
                hideAni.PlayFromStart();

            // Update remaining time label
            timeLabel.Text = (Mathf.Clamp(remainingTime, 0f, remainingTime) / 1000f).ToString("N1");
        }

        /// <summary>
        /// Event called on skip button trigger.
        /// </summary>
        private void OnSkipButton()
        {
            if(IsAnimating)
                return;

            GameSession.InvokeSkipped(skipToTime);
            hideAni.PlayFromStart();
        }

        /// <summary>
        /// Event called on game session hard initialization.
        /// </summary>
        private void OnHardInit()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Event called on game session soft initialization.
        /// </summary>
        private void OnSoftInit()
        {
            var map = GameSession.CurrentMap;
            var firstObject = map.HitObjects.FirstOrDefault();
            var firstTiming = map.ControlPoints.TimingPoints.FirstOrDefault();

            skipToTime = firstObject.StartTime - Mathf.Clamp(firstTiming.BeatLength * SkipBeatCount, MinSkipOffset, MaxSkipOffset);
            if (skipToTime < 0f)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            showAni.PlayFromStart();
        }

        /// <summary>
        /// Event called on game session soft disposal.
        /// </summary>
        private void OnSoftDispose()
        {
            skipToTime = 0f;
            gameObject.SetActive(false);
        }
    }
}