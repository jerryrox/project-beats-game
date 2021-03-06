using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Assets.Caching;
using PBGame.Rulesets;
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

        private ITimer searchScheduler;
        private string scheduledTerm;

        private IMapset mapsetForDropdown;
        private DropdownContext dropdownContext;

        private Bindable<List<IMapset>> mapsets = new Bindable<List<IMapset>>(new List<IMapset>());


        /// <summary>
        /// Returns the current mapset sorting type.
        /// </summary>
        public IReadOnlyBindable<MapsetSortType> SortType => GameConfiguration.MapsetSort;

        /// <summary>
        /// Returns whether unicode text should be preferred.
        /// </summary>
        public IReadOnlyBindable<bool> PreferUnicode => GameConfiguration.PreferUnicode;

        /// <summary>
        /// Returns the list of maps that should be displayed in the list.
        /// </summary>
        public IReadOnlyBindable<List<IMapset>> Mapsets => mapsets;

        /// <summary>
        /// Returns the currently selected mapset.
        /// </summary>
        public IReadOnlyBindable<IMapset> SelectedMapset => MapSelection.Mapset;

        /// <summary>
        /// Returns the currently selected game mode.
        /// </summary>
        public IReadOnlyBindable<GameModeType> GameMode => GameConfiguration.RulesetMode;

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

        /// <summary>
        /// Sets the sorting method of the mapsets.
        /// </summary>
        public void SetSort(MapsetSortType sort)
        {
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
        /// Selects the specified mapset.
        /// </summary>
        public void SelectMapset(IMapset mapset) => MapSelection.SelectMapset(mapset);

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

        protected override void OnPreShow()
        {
            base.OnPreShow();

            MapManager.DisplayedMapsets.OnChange += OnDisplayedMapsetChange;

            ApplySearch(LastSearchTerm);
            OnDisplayedMapsetChange(MapManager.DisplayedMapsets.RawList);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            MapManager.DisplayedMapsets.OnChange -= OnDisplayedMapsetChange;
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
        private void OnSearchSchedulerEnd()
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
                    bool preferUnicode = PreferUnicode.Value;
                    string artist = mapsetForDropdown.Metadata.GetArtist(preferUnicode);
                    string title = mapsetForDropdown.Metadata.GetTitle(preferUnicode);
                    // Confirm with the user first.
                    var dialogModel = OverlayNavigator.Show<DialogOverlay>().Model;
                    dialogModel.SetMessage($"Are you sure you want to delete this mapset?\n{artist} - {title}");
                    dialogModel.AddConfirmCancel(
                        onConfirm: () => MapManager.DeleteMapset(mapsetForDropdown)
                    );
                    break;

                case SongOffsetAction:
                    var offsetsModel = OverlayNavigator.Show<OffsetsOverlay>().Model;
                    offsetsModel.SetMap(mapsetForDropdown);
                    break;
            }
        }

        /// <summary>
        /// Event called on displayed mapsets list change.
        /// </summary>
        private void OnDisplayedMapsetChange(List<IMapset> mapsets)
        {
            this.mapsets.ModifyValue(list =>
            {
                list.Clear();
                list.AddRange(mapsets);
            });
        }
    }
}