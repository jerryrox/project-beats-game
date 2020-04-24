using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Networking.API;
using PBGame.Networking.Maps;
using PBFramework.Data.Bindables;

namespace PBGame.UI.Components.Download
{
    /// <summary>
    /// Represents the state within a DownloadScreen context.
    /// </summary>
    public class DownloadState {

        /// <summary>
        /// Returns the current API provider type.
        /// </summary>
        public Bindable<ApiProviders> ApiProvider { get; } = new Bindable<ApiProviders>(ApiProviders.Osu);

        /// <summary>
        /// Returns the mapset currently in preview.
        /// </summary>
        public Bindable<OnlineMapset> PreviewingMapset { get; } = new Bindable<OnlineMapset>();

        /// <summary>
        /// Returns the value of cursor.
        /// Depends on the sorting method.
        /// </summary>
        public BindableFloat CursorValue { get; } = new BindableFloat();

        /// <summary>
        /// Map identifier the cursor is on.
        /// </summary>
        public BindableInt CursorId { get; } = new BindableInt();

        /// <summary>
        /// Current game mode filter.
        /// </summary>
        public Bindable<GameModes> Mode { get; } = new Bindable<GameModes>(GameModes.OsuStandard);

        /// <summary>
        /// Current map category filter.
        /// </summary>
        public Bindable<MapCategories> Category { get; } = new Bindable<MapCategories>(MapCategories.Any);

        /// <summary>
        /// Current map genre filter.
        /// </summary>
        public Bindable<MapGenres> Genre { get; } = new Bindable<MapGenres>(MapGenres.Any);

        /// <summary>
        /// Current language filter.
        /// </summary>
        public Bindable<MapLanguages> Language { get; } = new Bindable<MapLanguages>(MapLanguages.Any);

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