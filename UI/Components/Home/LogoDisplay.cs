using System;
using PBGame.UI.Models;
using PBGame.Audio;
using PBGame.Animations;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Home
{
    public class LogoDisplay : Components.LogoDisplay {

        /// <summary>
        /// Event caled on logo press.
        /// </summary>
        public event Action OnPress;

        private IAnime pulseAni;
        private IAnime pointerEnterAni;
        private IAnime pointerExitAni;
        private IAnime zoomInAni;
        private IAnime zoomOutAni;

        private UguiTrigger trigger;

        /// <summary>
        /// The duration of a single pulse.
        /// </summary>
        public float PulseDuration
        {
            get => 1f / pulseAni.Speed;
            set => pulseAni.Speed = 1f / value;
        }

        [ReceivesDependency]
        private HomeModel Model { get; set; }

        [ReceivesDependency]
        private ISoundPool SoundPool { get; set; }


        [InitWithDependency]
        private void Init(IAnimePreset animePreset)
        {
            pulseAni = new Anime();
            pulseAni.AnimateVector3((scale) => this.Scale = scale)
                .AddTime(0f, new Vector3(1.1f, 1.1f, 1f), EaseType.SineEaseOut)
                .AddTime(1.5f, Vector3.one)
                .Build();

            pointerEnterAni = new Anime();
            pointerEnterAni.AnimateFloat((alpha) => this.Alpha = alpha)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.5f, 0.5f)
                .Build();

            pointerExitAni = new Anime();
            pointerExitAni.AnimateFloat((alpha) => this.Alpha = alpha)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.5f, 1f)
                .Build();

            zoomInAni = new Anime();
            zoomInAni.AnimateVector2((size) => this.Size = size)
                .AddTime(0f, () => this.Size, EaseType.QuartEaseIn)
                .AddTime(0.35f, new Vector2(700f, 700f))
                .Build();

            zoomOutAni = new Anime();
            zoomOutAni.AnimateVector2((size) => this.Size = size)
                .AddTime(0f, () => this.Size, EaseType.QuartEaseIn)
                .AddTime(0.5f, new Vector2(352f, 352f))
                .Build();

            pointerExitAni.PlayFromStart();

            trigger = CreateChild<UguiTrigger>("trigger", 1000);
            {
                trigger.Anchor = AnchorType.Fill;
                trigger.RawSize = Vector2.zero;

                trigger.OnPointerEnter += () =>
                {
                    pointerExitAni.Stop();
                    pointerEnterAni.PlayFromStart();

                    SoundPool.Play("menuhit");
                };
                trigger.OnPointerExit += () =>
                {
                    pointerEnterAni.Stop();
                    pointerExitAni.PlayFromStart();
                };
                trigger.OnPointerDown += () =>
                {
                    OnPress?.Invoke();

                    SoundPool.Play("menuclick");
                };
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.OnBeat += PlayPulse;
            Model.BeatSpeed.BindAndTrigger(OnBeatSpeedChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.OnBeat -= PlayPulse;
            Model.BeatSpeed.OnNewValue -= OnBeatSpeedChange;

            // The logo should be shrunk to its original size on disable.
            zoomOutAni.Seek(zoomOutAni.Duration);
        }

        /// <summary>
        /// Sets the progress of the pulse.
        /// </summary>
        public void SetPulseProgress(float progress)
        {
            pulseAni.Seek(progress);
        }

        /// <summary>
        /// Starts playing the pulsating animation.
        /// </summary>
        public void PlayPulse() => pulseAni.PlayFromStart();

        /// <summary>
        /// Stops playing the pulsating animation.
        /// </summary>
        public void StopPulse() => pulseAni.Stop();

        /// <summary>
        /// Sets zoom effect on the logo.
        /// </summary>
        public void SetZoom(bool isZoom)
        {
            if (isZoom)
            {
                zoomOutAni.Stop();
                zoomInAni.PlayFromStart();
            }
            else
            {
                zoomInAni.Stop();
                zoomOutAni.PlayFromStart();
            }
        }

        /// <summary>
        /// Event called when the metronome's beat speed changes.
        /// </summary>
        private void OnBeatSpeedChange(float speed)
        {
            pulseAni.Speed = speed;
        }
    }
}