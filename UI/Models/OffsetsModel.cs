using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Audio;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBGame.Configurations.Maps;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class OffsetsModel : BaseModel
    {
        private IMetronome metronome;

        private Bindable<MapsetConfig> mapsetConfig = new Bindable<MapsetConfig>();
        private Bindable<MapConfig> mapConfig = new Bindable<MapConfig>();
        private BindableBool isMetronomeAvailable = new BindableBool(false);

        private Coroutine updateRoutine;


        /// <summary>
        /// Returns the metronome.
        /// </summary>
        public IMetronome Metronome => metronome;

        /// <summary>
        /// Returns the configuration for the currently selected mapset.
        /// </summary>
        public IReadOnlyBindable<MapsetConfig> MapsetConfig => mapsetConfig;

        /// <summary>
        /// Returns the configuration for the currently selected map.
        /// </summary>
        public IReadOnlyBindable<MapConfig> MapConfig => mapConfig;

        /// <summary>
        /// Returns whether the metronome is available in current state.
        /// </summary>
        public IReadOnlyBindable<bool> IsMetronomeAvailable => isMetronomeAvailable;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMapsetConfiguration MapsetConfiguration { get; set; }

        [ReceivesDependency]
        private IMapConfiguration MapConfiguration { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }


        [InitWithDependency]
        private void Init()
        {
            metronome = new Metronome();
        }

        /// <summary>
        /// Sets the metronome beat frequency.
        /// </summary>
        public void SetFrequency(BeatFrequency frequency)
        {
            metronome.Frequency.Value = frequency;
        }

        /// <summary>
        /// Hides the offsets overlay.
        /// </summary>
        public void CloseOffsets()
        {
            OverlayNavigator.Hide<OffsetsOverlay>();
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            StartUpdate();

            MapSelection.Mapset.BindAndTrigger(OnMapsetChange);
            MapSelection.Map.BindAndTrigger(OnMapChange);

            ScreenNavigator.CurrentScreen.BindAndTrigger(OnScreenChange);

            IsMetronomeAvailable.BindAndTrigger(OnMetronomeAvailable);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            MapSelection.Mapset.OnNewValue -= OnMapsetChange;
            MapSelection.Map.OnNewValue -= OnMapChange;

            ScreenNavigator.CurrentScreen.OnNewValue -= OnScreenChange;

            IsMetronomeAvailable.OnNewValue -= OnMetronomeAvailable;

            DisposeMapsetConfig();
            DisposeMapConfig();
        }

        protected override void Update()
        {
            metronome.Update();
        }

        /// <summary>
        /// Disposes currently selected mapset's configuration.
        /// </summary>
        private void DisposeMapsetConfig()
        {
            var config = mapsetConfig.Value;
            if (config != null)
                MapsetConfiguration.SetConfig(config);
        }

        /// <summary>
        /// Disposes currently selected map's configuration.
        /// </summary>
        private void DisposeMapConfig()
        {
            var config = mapConfig.Value;
            if (config != null)
                MapConfiguration.SetConfig(config);
        }

        /// <summary>
        /// Event called when the selected mapset has changed.
        /// </summary>
        private void OnMapsetChange(IMapset mapset)
        {
            DisposeMapsetConfig();
            mapsetConfig.Value = MapsetConfiguration.GetConfig(mapset);
        }

        /// <summary>
        /// Event called when the selected map has changed.
        /// </summary>
        private void OnMapChange(IPlayableMap map)
        {
            DisposeMapConfig();
            mapConfig.Value = MapConfiguration.GetConfig(map);

            metronome.CurrentMap = map;
        }

        /// <summary>
        /// Event called when the screen has changed.
        /// </summary>
        private void OnScreenChange(INavigationView view)
        {
            isMetronomeAvailable.Value = !(view is GameScreen);
        }

        /// <summary>
        /// Event called on metronome availability change.
        /// </summary>
        private void OnMetronomeAvailable(bool isAvailable)
        {
            if(isAvailable)
                metronome.AudioController = MusicController;
            else
                metronome.AudioController = null;
        }
    }
}