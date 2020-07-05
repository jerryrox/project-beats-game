using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Configurations;
using PBFramework.UI;
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
        /// Amount of delay to add before automatically filtering the results even when not submitted.
        /// </summary>
        private const float SearchDelay = 1f;

        private Bindable<MapsetSortType> sortType = new Bindable<MapsetSortType>(MapsetSortType.Title);

        private ITimer searchScheduler;
        private string scheduledTerm;


        /// <summary>
        /// Returns the current mapset sorting type.
        /// </summary>
        public IReadOnlyBindable<MapsetSortType> SortType => sortType;

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


        [InitWithDependency]
        private void Init()
        {
            // Initilaize scheduler.
            searchScheduler = new SynchronizedTimer()
            {
                Limit = SearchDelay
            };
            searchScheduler.OnFinished += OnSearchSchedulerEnd;

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
    }
}