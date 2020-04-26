using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Networking.API;
using PBGame.Networking.API.Requests;
using PBGame.Networking.Maps;
using PBFramework.Data.Bindables;

namespace PBGame.UI.Components.Download
{
    /// <summary>
    /// Represents the state within a DownloadScreen context.
    /// </summary>
    public class DownloadState {

        /// <summary>
        /// Event called on next page request.
        /// </summary>
        public event Action OnNextPage;


        /// <summary>
        /// The name of the cursor associated with cursor value.
        /// </summary>
        public string CursorName { get; set; }

        /// <summary>
        /// The value of cursor.
        /// </summary>
        public float CursorValue { get; set; }

        /// <summary>
        /// Map identifier the cursor is on.
        /// </summary>
        public int CursorId { get; set; }

        /// <summary>
        /// Returns the current search request in progress.
        /// </summary>
        public Bindable<IMapsetListRequest> SearchRequest { get; } = new Bindable<IMapsetListRequest>();

        /// <summary>
        /// Returns the current API provider type.
        /// </summary>
        public Bindable<ApiProviders> ApiProvider { get; } = new Bindable<ApiProviders>();

        /// <summary>
        /// Returns the mapset currently in preview.
        /// </summary>
        public Bindable<OnlineMapset> PreviewingMapset { get; } = new Bindable<OnlineMapset>();

        /// <summary>
        /// Returns the list of mapsets returned from search API.
        /// </summary>
        public Bindable<List<OnlineMapset>> Results { get; } = new Bindable<List<OnlineMapset>>(new List<OnlineMapset>());

        /// <summary>
        /// Current game mode filter.
        /// </summary>
        public Bindable<GameModes> Mode { get; } = new Bindable<GameModes>();

        /// <summary>
        /// Current map category filter.
        /// </summary>
        public Bindable<MapCategories> Category { get; } = new Bindable<MapCategories>();

        /// <summary>
        /// Current map genre filter.
        /// </summary>
        public Bindable<MapGenres> Genre { get; } = new Bindable<MapGenres>();

        /// <summary>
        /// Current language filter.
        /// </summary>
        public Bindable<MapLanguages> Language { get; } = new Bindable<MapLanguages>();

        /// <summary>
        /// Current rank state filter.
        /// </summary>
        public Bindable<MapStatus> RankState { get; } = new Bindable<MapStatus>();

        /// <summary>
        /// Sorting criteria.
        /// </summary>
        public Bindable<MapSortType> Sort { get; } = new Bindable<MapSortType>();

        /// <summary>
        /// Video mandatory inclusion filter.
        /// </summary>
        public BindableBool HasVideo { get; } = new BindableBool();

        /// <summary>
        /// Storyboard mandatory inclusion filter.
        /// </summary>
        public BindableBool HasStoryboard { get; } = new BindableBool();

        /// <summary>
        /// Whether result is in descending order.
        /// </summary>
        public BindableBool IsDescending { get; } = new BindableBool();

        /// <summary>
        /// Returns the current search term.
        /// </summary>
        public Bindable<string> SearchTerm { get; } = new Bindable<string>();



        public DownloadState() => ResetState();

        /// <summary>
        /// Resets state to inital values.
        /// </summary>
        public void ResetState()
        {
            CursorName = "";
            CursorValue = 0;
            CursorId = 0;
            SearchRequest.Value = null;
            ApiProvider.Value = ApiProviders.Osu;
            PreviewingMapset.Value = null;
            Results.Value.Clear();
            Mode.Value = GameModes.OsuStandard;
            Genre.Value = MapGenres.Any;
            Language.Value = MapLanguages.Any;
            RankState.Value = MapStatus.Ranked;
            Sort.Value = MapSortType.Ranked;
            HasVideo.Value = false;
            HasStoryboard.Value = false;
            IsDescending.Value = true;
            SearchTerm.Value = "";
        }

        /// <summary>
        /// Requests for the next page using the same options.
        /// </summary>
        public void RequestNextPage() => OnNextPage?.Invoke();

        /// <summary>
        /// Performs the specified action on given result list and triggers the bindable afterwards.
        /// </summary>
        public void ModifyResults(Action<List<OnlineMapset>> action)
        {
            action?.Invoke(Results.Value);
            Results.Trigger();
        }
    }
}