using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Data.Rankings;
using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Models
{
    public class PrepareModel : BaseModel {

        private BindableBool isDetailedMode = new BindableBool(false);
        private Bindable<List<IOriginalMap>> mapList = new Bindable<List<IOriginalMap>>();
        private Bindable<string> mapsetDescription = new Bindable<string>("");
        private Bindable<List<IRankInfo>> rankList = new Bindable<List<IRankInfo>>();


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
        /// Returns the currently selected rank display type.
        /// </summary>
        public IReadOnlyBindable<RankDisplayType> RankDisplay => GameConfiguration.RankDisplay;

        /// <summary>
        /// Returns the list of maps included in the selected mapset.
        /// </summary>
        public IReadOnlyBindable<List<IOriginalMap>> MapList => mapList;

        /// <summary>
        /// Returns the list of ranks loaded.
        /// </summary>
        public IReadOnlyBindable<List<IRankInfo>> RankList => rankList;

        /// <summary>
        /// Returns the description of the mapset.
        /// </summary>
        public IReadOnlyBindable<string> MapsetDescription => mapsetDescription;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }


        /// <summary>
        /// Toggles between detailed/brief information display mode.
        /// </summary>
        public void ToggleDetailedMode() => SetDetailedMode(!isDetailedMode.Value);

        /// <summary>
        /// Sets the detailed/brief display mode.
        /// </summary>
        public void SetDetailedMode(bool isDetailed) => isDetailedMode.Value = isDetailed;

        /// <summary>
        /// Selects the next map in the map list.
        /// </summary>
        public void SelectNextMap()
        {
            var maps = mapList.Value;
            int index = GetSelectedMapIndex() + 1;
            if(index >= maps.Count)
                MapSelection.SelectMap(maps[0]);
            else
                MapSelection.SelectMap(maps[index]);
        }

        /// <summary>
        /// Selects the previous map in the map list.
        /// </summary>
        public void SelectPrevMap()
        {
            var maps = mapList.Value;
            int index = GetSelectedMapIndex() - 1;
            if(index < 0)
                MapSelection.SelectMap(maps[maps.Count - 1]);
            else
                MapSelection.SelectMap(maps[index]);
        }

        /// <summary>
        /// Navigates back to songs screen.
        /// </summary>
        public void NavigateToSongs() => ScreenNavigator.Show<SongsScreen>();

        /// <summary>
        /// Navigates away toward game screen.
        /// </summary>
        public void NavigateToGame()
        {
            ScreenNavigator.Hide<PrepareScreen>();
            OverlayNavigator.Show<GameLoadOverlay>();
        }

        /// <summary>
        /// Sets the type of rank display source.
        /// </summary>
        public void SetRankDisplay(RankDisplayType type)
        {
            GameConfiguration.RankDisplay.Value = type;
            GameConfiguration.Save();
        }

        /// <summary>
        /// Returns the index of the selected map.
        /// </summary>
        public int GetSelectedMapIndex()
        {
            var maps = mapList.Value;
            var curMap = SelectedMap.Value.OriginalMap;
            return maps.IndexOf(curMap);
        }

        /// <summary>
        /// Returns the mode servicer for currently selected game mode.
        /// </summary>
        public IModeService GetSelectedModeService() => ModeManager.GetService(GameMode.Value);

        protected override void OnPreShow()
        {
            base.OnPreShow();

            SelectedMapset.OnNewValue += OnMapsetChange;
            SelectedMap.OnNewValue += OnMapChange;
            GameMode.OnNewValue += OnGameModeChange;
            RankDisplay.OnNewValue += OnRankDisplayChange;

            SetDetailedMode(false);
            SetMapList();
            RequestMapsetDescription();
            RequestRankings();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            SelectedMapset.OnNewValue -= OnMapsetChange;
            SelectedMap.OnNewValue -= OnMapChange;
            GameMode.OnNewValue -= OnGameModeChange;
            RankDisplay.OnNewValue -= OnRankDisplayChange;

            mapsetDescription.Value = "";
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
        /// Starts requesting for selected mapset's description from API.
        /// </summary>
        private void RequestMapsetDescription()
        {
            mapsetDescription.Value = "";
            // TODO:
        }

        /// <summary>
        /// Requests for the ranking list.
        /// </summary>
        private void RequestRankings()
        {
            rankList.Value = new List<IRankInfo>();
            // TODO:
        }

        /// <summary>
        /// Event called on current map change.
        /// </summary>
        private void OnMapChange(IPlayableMap map)
        {
            RequestRankings();
        }

        /// <summary>
        /// Event called on current mapset change.
        /// </summary>
        private void OnMapsetChange(IMapset mapset)
        {
            SetMapList();
            RequestMapsetDescription();
        }

        /// <summary>
        /// Event called on game mode change.
        /// </summary>
        private void OnGameModeChange(GameModeType mode)
        {
            SetMapList();
            RequestRankings();
        }

        /// <summary>
        /// Event clled on rank display change.
        /// </summary>
        private void OnRankDisplayChange(RankDisplayType type)
        {
            RequestRankings();
        }
    }
}