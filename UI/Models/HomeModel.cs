using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class HomeModel : BaseModel {

        /// <summary>
        /// Event called on metronome beat callback.
        /// </summary>
        public event Action OnBeat;

        private BindableBool isHomeMenuShown = new BindableBool(false);
        private BindableFloat beatSpeed = new BindableFloat(1f);


        /// <summary>
        /// Returns whether the home menu overlay is currently shown.
        /// </summary>
        public IReadOnlyBindable<bool> IsHomeMenuShown => isHomeMenuShown;

        /// <summary>
        /// The speed scale of the metronome beat.
        /// </summary>
        public IReadOnlyBindable<float> BeatSpeed => beatSpeed;

        /// <summary>
        /// Returns the BackgroundOverlay instance from the navigator.
        /// </summary>
        private BackgroundOverlay BgOverlay => OverlayNavigator.Get<BackgroundOverlay>();

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMetronome Metronome { get; set; }


        [InitWithDependency]
        private void Init()
        {
            // Initially select a random song.
            SelectRandomMapset();
        }

        /// <summary>
        /// Shows the home menu overlay.
        /// </summary>
        public void ShowHomeMenuOverlay()
        {
            if(isHomeMenuShown.Value)
                return;

            var homeMenuOverlay = OverlayNavigator.Show<HomeMenuOverlay>();
            if (homeMenuOverlay != null)
            {
                isHomeMenuShown.Value = true;
                BgOverlay.Color = Color.gray;

                homeMenuOverlay.OnHide += OnHomeMenuHide;
            }
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            Metronome.OnBeat += OnMetronomeBeat;
            Metronome.BeatLength.BindAndTrigger(OnBeatLengthChange);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            Metronome.OnBeat -= OnMetronomeBeat;
            Metronome.BeatLength.OnNewValue -= OnBeatLengthChange;

            OverlayNavigator.Hide<HomeMenuOverlay>();
        }

        /// <summary>
        /// Selects a random mapset within the map manager.
        /// </summary>
        private void SelectRandomMapset()
        {
            // Try get a random mapset.
            var mapset = MapManager.AllMapsets.GetRandom();
            if (mapset != null)
            {
                // Select the mapset.
                MapSelection.SelectMapset(mapset);
            }
        }

        /// <summary>
        /// Event called from HomeMenuOverlay when it has become hidden.
        /// </summary>
        private void OnHomeMenuHide()
        {
            isHomeMenuShown.Value = false;
            BgOverlay.Color = Color.white;
        }

        /// <summary>
        /// Event called when the metronome beats.
        /// </summary>
        private void OnMetronomeBeat()
        {
            OnBeat?.Invoke();
        }

        /// <summary>
        /// Event called when the metronome's beat length changes.
        /// </summary>
        private void OnBeatLengthChange(float beatLength)
        {
            beatSpeed.Value = 1000f / beatLength;
        }
    }
}