using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Networking.API;
using PBGame.Networking.Maps;
using PBFramework.Data.Bindables;

namespace PBGame.UI.Models.Download
{
    public class SearchOptions {

        /// <summary>
        /// Returns whether there is a valid cursor for querying the next set of mapsets using the same options.
        /// </summary>
        public bool HasCursor => !string.IsNullOrEmpty(Cursor);

        /// <summary>
        /// The current search cursor.
        /// </summary>
        public string Cursor { get; set; } = null;

        /// <summary>
        /// Returns the current API provider type.
        /// </summary>
        public Bindable<ApiProviderType> ApiProvider { get; } = new Bindable<ApiProviderType>(ApiProviderType.Osu);

        /// <summary>
        /// Current game mode filter.
        /// </summary>
        public Bindable<GameModeType> Mode { get; } = new Bindable<GameModeType>(GameModeType.OsuStandard);

        /// <summary>
        /// Current map category filter.
        /// </summary>
        public Bindable<MapCategoryType> Category { get; } = new Bindable<MapCategoryType>(MapCategoryType.Ranked);

        /// <summary>
        /// Current map genre filter.
        /// </summary>
        public Bindable<MapGenreType> Genre { get; } = new Bindable<MapGenreType>(MapGenreType.Any);

        /// <summary>
        /// Current language filter.
        /// </summary>
        public Bindable<MapLanguageType> Language { get; } = new Bindable<MapLanguageType>(MapLanguageType.Any);

        /// <summary>
        /// Sorting criteria.
        /// </summary>
        public Bindable<MapSortType> Sort { get; } = new Bindable<MapSortType>(MapSortType.Ranked);

        /// <summary>
        /// Video mandatory inclusion filter.
        /// </summary>
        public BindableBool HasVideo { get; } = new BindableBool(false);

        /// <summary>
        /// Storyboard mandatory inclusion filter.
        /// </summary>
        public BindableBool HasStoryboard { get; } = new BindableBool(false);

        /// <summary>
        /// Whether result is in descending order.
        /// </summary>
        public BindableBool IsDescending { get; } = new BindableBool(true);

        /// <summary>
        /// Returns the current search term.
        /// </summary>
        public Bindable<string> SearchTerm { get; } = new Bindable<string>("");
    }
}