using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class PrepareModel : BaseModel {

        private BindableBool isDetailedMode = new BindableBool(false);
        private Bindable<List<IOriginalMap>> mapList = new Bindable<List<IOriginalMap>>();


        /// <summary>
        /// Returns whether the map information should be displayed as detailed mode.
        /// </summary>
        public IReadOnlyBindable<bool> IsDetailedMode => isDetailedMode;

        /// <summary>
        /// Returns the currently selected map.
        /// </summary>
        public IReadOnlyBindable<IPlayableMap> SelectedMap => MapSelection.Map;

        /// <summary>
        /// Returns the currently selected mapset.
        /// </summary>
        public IReadOnlyBindable<IMapset> SelectedMapset => MapSelection.Mapset;

        /// <summary>
        /// Returns whether unicode is preferred.
        /// </summary>
        public IReadOnlyBindable<bool> PreferUnicode => GameConfiguration.PreferUnicode;

        /// <summary>
        /// Returns the currently selected game mode.
        /// </summary>
        public IReadOnlyBindable<GameModeType> GameMode => GameConfiguration.RulesetMode;

        /// <summary>
        /// Returns the list of maps included in the selected mapset.
        /// </summary>
        public IReadOnlyBindable<List<IOriginalMap>> MapList => mapList;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        /// <summary>
        /// Toggles between detailed/brief information display mode.
        /// </summary>
        public void ToggleDetailedMode() => SetDetailedMode(!isDetailedMode.Value);

        /// <summary>
        /// Sets the detailed/brief display mode.
        /// </summary>
        public void SetDetailedMode(bool isDetailed) => isDetailedMode.Value = isDetailed;

        protected override void OnPreShow()
        {
            base.OnPreShow();

            SelectedMapset.BindAndTrigger(OnMapsetChange);
            GameMode.OnNewValue += OnGameModeChange;

            SetDetailedMode(false);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            SelectedMapset.OnNewValue -= OnMapsetChange;
            GameMode.OnNewValue -= OnGameModeChange;
        }

        /// <summary>
        /// Sets map list for specified mapset and sorts by current game mode.
        /// </summary>
        private void SetMapList()
        {
            var mapset = SelectedMapset.Value;
            if (mapset != null)
            {
                // Sort maps
                mapset.SortMapsByMode(GameMode.Value);
                mapList.Value = mapset.Maps;
            }
        }

        /// <summary>
        /// Event called on current mapset change.
        /// </summary>
        private void OnMapsetChange(IMapset mapset) => SetMapList();

        /// <summary>
        /// Event called on game mode change.
        /// </summary>
        private void OnGameModeChange(GameModeType mode) => SetMapList();
    }
}