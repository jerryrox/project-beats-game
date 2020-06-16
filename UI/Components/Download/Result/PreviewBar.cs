using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Result
{
    public class PreviewBar : UguiProgressBar, IHasAlpha {

        private CanvasGroup canvasGroup;

        private IAnime showAni;
        private IAnime hideAni;

        private bool isShowing;
        private OnlineMapset mapset;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        /// <summary>
        /// Returns whether the preview bar should be displayed.
        /// </summary>
        private bool ShouldDisplay => mapset != null && State.PreviewingMapset.Value == mapset;

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private DownloadState State { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            canvasGroup = RawObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;

            background.Color = Color.black;
            foreground.Color = colorPreset.SecondaryFocus;

            showAni = new Anime();
            showAni.AnimateFloat(a => Alpha = a)
                .AddTime(0f, () => Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat(a => Alpha = a)
                .AddTime(0f, () => Alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Reset();
        }

        /// <summary>
        /// Initializes the music preview bar for specified mapset.
        /// </summary>
        public void Setup(OnlineMapset mapset)
        {
            Reset();
            if(mapset == null)
                return;

            this.mapset = mapset;

            bool isPreviewingMapset = ShouldDisplay;
            State.PreviewingMapset.OnNewValue += OnPreviewMapsetChange;
            Toggle(isPreviewingMapset, false);

            Value = isPreviewingMapset ? MusicController.Progress : 0f;
        }

        /// <summary>
        /// Hides the preview time bar.
        /// </summary>
        public void Reset()
        {
            showAni.Stop();
            hideAni.Stop();

            if(mapset == null)
                return;

            State.PreviewingMapset.OnNewValue -= OnPreviewMapsetChange;

            mapset = null;
            Value = 0f;
            Alpha = 0f;
        }

        protected void Update()
        {
            if(mapset == null || State.PreviewingMapset.Value != mapset)
                return;

            Value = MusicController.Progress;
        }

        /// <summary>
        /// Toggles preview bar visibility.
        /// </summary>
        private void Toggle(bool show, bool animate)
        {
            if(isShowing == show)
                return;
            isShowing = show;

            showAni.Stop();
            hideAni.Stop();

            if (show)
            {
                if(animate)
                    showAni.PlayFromStart();
                else
                    showAni.Seek(showAni.Duration);
            }
            else
            {
                if(animate)
                    hideAni.PlayFromStart();
                else
                    hideAni.Seek(hideAni.Duration);
            }
        }

        /// <summary>
        /// Event called on previewing mapset change.
        /// </summary>
        private void OnPreviewMapsetChange(OnlineMapset mapset)
        {
            Toggle(ShouldDisplay, true);
        }
    }
}