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
        /// Event called on fresh list request.
        /// </summary>
        public event Action OnRequestList;


        /// <summary>
        /// The current search cursor.
        /// </summary>
        public string Cursor { get; set; }

        /// <summary>
        /// Returns whether the search request is requesting for the next page.
        /// </summary>
        public bool IsRequestingNextPage { get; private set; }

        /// <summary>
        /// Returns the current search request in progress.
        /// </summary>
        public Bindable<MapsetsRequest> SearchRequest { get; } = new Bindable<MapsetsRequest>();

        /// <summary>
        /// Returns the current API provider type.
        /// </summary>
        public Bindable<ApiProviderType> ApiProvider { get; } = new Bindable<ApiProviderType>();

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
        public Bindable<GameModeType> Mode { get; } = new Bindable<GameModeType>();

        /// <summary>
        /// Current map category filter.
        /// </summary>
        public Bindable<MapCategoryType> Category { get; } = new Bindable<MapCategoryType>();

        /// <summary>
        /// Current map genre filter.
        /// </summary>
        public Bindable<MapGenreType> Genre { get; } = new Bindable<MapGenreType>();

        /// <summary>
        /// Current language filter.
        /// </summary>
        public Bindable<MapLanguageType> Language { get; } = new Bindable<MapLanguageType>();

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



        public DownloadState()
        {
            ResetState();

            // Trigger mapset list request when any of these values change.
            Mode.OnValueChanged += delegate { RequestMapsetList(); };
            Category.OnValueChanged += delegate { RequestMapsetList(); };
            Genre.OnValueChanged += delegate { RequestMapsetList(); };
            Language.OnValueChanged += delegate { RequestMapsetList(); };
            Sort.OnValueChanged += delegate { RequestMapsetList(); };
            HasVideo.OnValueChanged += delegate { RequestMapsetList(); };
            HasStoryboard.OnValueChanged += delegate { RequestMapsetList(); };
            IsDescending.OnValueChanged += delegate { RequestMapsetList(); };
            SearchTerm.OnValueChanged += delegate { RequestMapsetList(); };
        }

        /// <summary>
        /// Resets state to inital values.
        /// </summary>
        public void ResetState()
        {
            Cursor = null;
            IsRequestingNextPage = false;
            SearchRequest.Value = null;
            ApiProvider.Value = ApiProviderType.Osu;
            PreviewingMapset.Value = null;
            Results.Value.Clear();
            Mode.Value = GameModeType.OsuStandard;
            Category.Value = MapCategoryType.Ranked;
            Genre.Value = MapGenreType.Any;
            Language.Value = MapLanguageType.Any;
            Sort.Value = MapSortType.Ranked;
            HasVideo.Value = false;
            HasStoryboard.Value = false;
            IsDescending.Value = true;
            SearchTerm.Value = "";
        }

        /// <summary>
        /// Requests for fresh mapset list.
        /// </summary>
        public void RequestMapsetList()
        {
            IsRequestingNextPage = false;
            Cursor = null;
            OnRequestList?.Invoke();
        }

        /// <summary>
        /// Requests for the next page using the same options.
        /// </summary>
        public void RequestNextPage()
        {
            IsRequestingNextPage = true;
            OnNextPage?.Invoke();
        }

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