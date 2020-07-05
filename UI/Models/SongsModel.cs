using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class SongsModel : BaseModel {

        /// <summary>
        /// Deletion action represented in the dropdown menu.
        /// </summary>
        private const int SongDeleteAction = 0;

        /// <summary>
        /// Offset action represented in the dropdown menu.
        /// </summary>
        private const int SongOffsetAction = 1;

        /// <summary>
        /// Amount of delay to add before automatically filtering the results even when not submitted.
        /// </summary>
        private const float SearchDelay = 1f;

        private Bindable<MapsetSortType> sortType = new Bindable<MapsetSortType>(MapsetSortType.Title);

        private ITimer searchScheduler;
        private string scheduledTerm;

        private Bindable<List<IMapset>> mapsets;

        private IMapset mapsetForDropdown;
        private DropdownContext dropdownContext;


        /// <summary>
        /// Returns the current mapset sorting type.
        /// </summary>
        public IReadOnlyBindable<MapsetSortType> SortType => sortType;

        /// <summary>
        /// Returns the list of maps that should be displayed in the list.
        /// </summary>
        public IReadOnlyBindable<List<IMapset>> Mapsets => mapsets;

        /// <summary>
        /// Returns the currently selected mapset.
        /// </summary>
        public IReadOnlyBindable<IMapset> SelectedMapset => MapSelection.Mapset;

        /// <summary>
        /// Returns the last search term used for filtering.
        /// </summary>
        public string LastSearchTerm => MapManager.LastSearch;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IDropdownProvider DropdownProvider { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            // Initilaize scheduler.
            searchScheduler = new SynchronizedTimer()
            {
                Limit = SearchDelay
            };
            searchScheduler.OnFinished += OnSearchSchedulerEnd;

            // Init dropdown context.
            dropdownContext = new DropdownContext() { IsSelectionMenu = false };
            dropdownContext.OnSelection += OnDropdownSelection;
            dropdownContext.Datas.Add(new DropdownData("Offset", SongOffsetAction));
            dropdownContext.Datas.Add(new DropdownData("Delete", SongDeleteAction));

            // Set initial selection
            SetSort(GameConfiguration.MapsetSort.Value);
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();
            ApplySearch(LastSearchTerm);
        }

        /// <summary>
        /// Sets the sorting method of the mapsets.
        /// </summary>
        public void SetSort(MapsetSortType sort)
        {
            sortType.Value = sort;

            // Save to configuration.
            if (GameConfiguration.MapsetSort.Value != sort)
            {
                GameConfiguration.MapsetSort.Value = sort;
                GameConfiguration.Save();
            }
        }

        /// <summary>
        /// Applies search query on the map manager.
        /// </summary>
        public void ApplySearch(string value)
        {
            StopScheduledSearch();

            if (MapManager.LastSearch != value)
                MapManager.Search(value);

            // If only a single result, select it.
            if(MapManager.DisplayedMapsets.Count == 1)
                MapSelection.SelectMapset(MapManager.DisplayedMapsets[0]);
        }

        /// <summary>
        /// Schedules searching after a certain amount of delay.
        /// </summary>
        public void ScheduleSearch(string value)
        {
            StopScheduledSearch();

            scheduledTerm = value ?? "";
            searchScheduler.Start();
        }

        /// <summary>
        /// Triggers the dropdown menu to show for specified mapset.
        /// </summary>
        public void TriggerDropdown(IMapset mapset, Vector2 worldPos)
        {
            if(mapset == null)
                return;

            mapsetForDropdown = mapset;
            DropdownProvider.OpenAt(dropdownContext, worldPos);
        }

        /// <summary>
        /// Selects a random mapset from the visible mapsets list.
        /// </summary>
        public void SelectRandomMapset() => MapSelection.SelectMapset(MapManager.DisplayedMapsets.GetRandom());

        /// <summary>
        /// Selects the previous mapset from the visible mapsets list relative to the current selection.
        /// </summary>
        public void SelectPrevMapset() => MapSelection.SelectMapset(MapManager.DisplayedMapsets.GetPrevious(SelectedMapset.Value));

        /// <summary>
        /// Selects the next mapset from the visible mapsets list relative to the current selection.
        /// </summary>
        public void SelectNextMapset() => MapSelection.SelectMapset(MapManager.DisplayedMapsets.GetNext(SelectedMapset.Value));

        /// <summary>
        /// Navigates away to prepare screen.
        /// </summary>
        public void NavigateToPrepare() => ScreenNavigator.Show<PrepareScreen>();

        /// <summary>
        /// Navigates away to home screen.
        /// </summary>
        public void NavigateToHome() => ScreenNavigator.Show<HomeScreen>();

        /// <summary>
        /// Returns the index of the currently selected mapset within the visible mapsets list.
        /// </summary>
        public int GetSelectedMapsetIndex()
        {
            return mapsets.Value.IndexOf(SelectedMapset.Value);
        }

        /// <summary>
        /// Stops any scheduled search.
        /// </summary>
        private void StopScheduledSearch()
        {
            searchScheduler.Stop();
            searchScheduler.Current = 0f;
            scheduledTerm = null;
        }

        /// <summary>
        /// Event called when the scheduled search should occur.
        /// </summary>
        private void OnSearchSchedulerEnd(ITimer timer)
        {
            if(scheduledTerm != null)
                ApplySearch(scheduledTerm);
        }

        /// <summary>
        /// Event called from dropdown context when a menu iten has been selected.
        /// </summary>
        private void OnDropdownSelection(DropdownData data)
        {
            if (mapsetForDropdown == null)
                return;

            int action = (int)data.ExtraData;
            switch (action)
            {
                case SongDeleteAction:
                    // TODO: Come back when mapset deletion is implemented.
                    Debug.LogWarning("Delete mapset: " + mapsetForDropdown.Metadata.Title);
                    break;
                case SongOffsetAction:
                    // If not the selected mapset, make it selected.
                    if(mapsetForDropdown != MapSelection.Mapset.Value)
                        MapSelection.SelectMapset(mapsetForDropdown);
                    // Now show the offsets overlay.
                    OverlayNavigator.Show<OffsetsOverlay>().Setup();
                    break;
            }
        }
    }
}