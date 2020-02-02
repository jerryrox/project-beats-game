using System;
using PBGame.Maps;
using PBGame.Audio;
using PBGame.Animations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Home
{
    public class LogoDisplay : Components.LogoDisplay, ILogoDisplay {

        public event Action OnPress;

        private IAnime pulseAni;
        private IAnime pointerEnterAni;
        private IAnime pointerExitAni;
        private IAnime zoomInAni;
        private IAnime zoomOutAni;

        private UguiTrigger trigger;

        public float PulseDuration
        {
            get => 1f / pulseAni.Speed;
            set => pulseAni.Speed = 1f / value;
        }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMetronome Metronome { get; set; }

        [ReceivesDependency]
        private ISoundPooler SoundPooler { get; set; }


        [InitWithDependency]
        private void Init(IAnimePreset animePreset)
        {
            pulseAni = animePreset.GetHomeLogoPulse(this);
            pointerEnterAni = animePreset.GetHomeLogoHover(this);
            pointerExitAni = animePreset.GetHomeLogoExit(this);
            zoomInAni = animePreset.GetHomeLogoZoomIn(this);
            zoomOutAni = animePreset.GetHomeLogoZoomOut(this);

            pointerExitAni.PlayFromStart();

            trigger = CreateChild<UguiTrigger>("trigger", 1000);
            {
                trigger.Anchor = Anchors.Fill;
                trigger.RawSize = Vector2.zero;
                
                trigger.OnPointerEnter += OnTriggerEntered;
                trigger.OnPointerExit += OnTriggerExited;
                trigger.OnPointerDown += OnTriggerPressed;
            }

            OnEnableInited();
        }

        public void SetPulseProgress(float progress)
        {
            pulseAni.Seek(progress);
        }

        public void PlayPulse()
        {
            pulseAni.PlayFromStart();
        }

        public void StopPulse()
        {
            pulseAni.Stop();
        }

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
        /// Event called from trigger when the cursor has entered.
        /// </summary>
        private void OnTriggerEntered()
        {
            pointerExitAni.Stop();
            pointerEnterAni.PlayFromStart();

            SoundPooler.Play("menuhit");
        }

        /// <summary>
        /// Event called from trigger when the cursor has exited.
        /// </summary>
        private void OnTriggerExited()
        {
            pointerEnterAni.Stop();
            pointerExitAni.PlayFromStart();
        }

        /// <summary>
        /// Event called from trigger when the cursor pressed it.
        /// </summary>
        private void OnTriggerPressed()
        {
            OnPress?.Invoke();

            SoundPooler.Play("menuclick");
        }

        /// <summary>
        /// Binds events to external dependencies.
        /// </summary>
        private void BindEvents()
        {
            Metronome.OnBeat += PlayPulse;
        }

        /// <summary>
        /// Unbinds events hooked to external dependencies.
        /// </summary>
        private void UnbindEvents()
        {
            Metronome.OnBeat -= PlayPulse;
        }

        protected override void OnEnableInited()
        {
            BindEvents();
        }

        protected override void OnDisable()
        {
            UnbindEvents();
        }
    }
}