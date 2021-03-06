using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.Download;
using PBGame.Maps;
using PBGame.Stores;
using PBGame.Assets.Caching;
using PBGame.Networking.API;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBGame.Networking.Maps;
using PBGame.Notifications;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Allocation.Caching;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class DownloadModel : BaseModel {

        /// <summary>
        /// Event called when the online mapset list has changed.
        /// The second option provides whether a search cursor was involved in this change.
        /// </summary>
        public event Action<List<OnlineMapset>, bool> OnMapsetListChange;

        /// <summary>
        /// Event called when the search list should be scrolled to top.
        /// </summary>
        public event Action OnScrollTopRequest;

        private CacherAgent<string, IMusicAudio> musicAgent;

        private Bindable<MapsetsRequest> mapsetsRequest = new Bindable<MapsetsRequest>();
        private Bindable<List<OnlineMapset>> mapsetList = new Bindable<List<OnlineMapset>>(new List<OnlineMapset>());
        private Bindable<OnlineMapset> previewingMapset = new Bindable<OnlineMapset>();
        private BindableBool isScrolledDown = new BindableBool(false)
        {
            TriggerWhenDifferent = true,
        };


        /// <summary>
        /// Returns the current mapset request.
        /// </summary>
        public IReadOnlyBindable<MapsetsRequest> MapsetsRequest => mapsetsRequest;

        /// <summary>
        /// Returns the list of mapsets returned by the API.
        /// </summary>
        public IReadOnlyBindable<List<OnlineMapset>> MapsetList => mapsetList;

        /// <summary>
        /// Returns the mapset currently playing the preview audio.
        /// </summary>
        public IReadOnlyBindable<OnlineMapset> PreviewingMapset => previewingMapset;

        /// <summary>
        /// Returns whether the songs list is currently scrolled down.
        /// </summary>
        public IReadOnlyBindable<bool> IsScrolledDown => isScrolledDown;

        /// <summary>
        /// Returns whether there is an on-going mapset request.
        /// </summary>
        public bool IsRequestingMapset => mapsetsRequest.Value != null;

        /// <summary>
        /// Returns the mapset search option holder instance.
        /// </summary>
        public SearchOptions Options { get; private set; }

        /// <summary>
        /// Returns the api provider instance related to current option's provider.
        /// </summary>
        public IApiProvider SelectedProvider => Api.GetProvider(Options.ApiProvider.Value);

        [ReceivesDependency]
        private IWebMusicCacher MusicCacher { get; set; }

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IApi Api { get; set; }

        [ReceivesDependency]
        private INotificationBox NotificationBox { get; set; }

        [ReceivesDependency]
        private IDownloadStore DownloadStore { get; set; }


        [InitWithDependency]
        private void Init()
        {
            // Initialize music cacher agent.
            musicAgent = new CacherAgent<string, IMusicAudio>(MusicCacher);
            musicAgent.OnFinished += OnMusicAudioLoaded;

            ResetOptions();
        }

        /// <summary>
        /// Starts requesting mapset list via API.
        /// </summary>
        public void RequestMapsets()
        {
            StopMapsetRequest();

            // Ensure there is a valid provider to process this request.
            var provider = Api.GetProvider(Options.ApiProvider.Value);
            if(provider == null)
                return;

            // Apply search options.
            var request = provider.Mapsets();
            request.Cursor = Options.Cursor;
            request.GameMode = Options.Mode.Value;
            request.Category = Options.Category.Value;
            request.Genre = Options.Genre.Value;
            request.Language = Options.Language.Value;
            request.Sort = Options.Sort.Value;
            request.IsDescending = Options.IsDescending.Value;
            request.HasVideo = Options.HasVideo.Value;
            request.HasStoryboard = Options.HasStoryboard.Value;
            request.Query = Options.SearchTerm.Value;
            request.Response.OnNewValue += OnMapsetsResponse;

            // Start requesting
            mapsetsRequest.Value = request;
            Api.Request(request);
        }

        /// <summary>
        /// Makes the specified mapset the previewing mapset.
        /// </summary>
        public void SetPreview(OnlineMapset mapset)
        {
            bool isPreviewing = mapset == previewingMapset.Value;
            ResetPreviewingMapset();

            if (!isPreviewing && mapset != null && !string.IsNullOrEmpty(mapset.PreviewAudio))
            {
                previewingMapset.Value = mapset;
                musicAgent.Request(mapset.PreviewAudio);
            }
        }

        /// <summary>
        /// Starts downloading the specified mapset.
        /// </summary>
        public void DownloadMapset(OnlineMapset mapset)
        {
            if(mapset == null)
                return;

            // Ensure the provider exists
            var provider = SelectedProvider;
            if(provider == null)
                return;

            // Setup request.
            var request = provider.MapsetDownload();
            request.DownloadStore = DownloadStore;
            request.Mapset = mapset;

            // Start request
            Api.Request(request);
        }

        /// <summary>
        /// Returns whether the specified mapset is a previewing mapset.
        /// </summary>
        public bool IsPreviewingMapset(OnlineMapset mapset)
        {
            return mapset != null && mapset == previewingMapset.Value;
        }

        /// <summary>
        /// Returns the music preview progress value for specified mapset.
        /// </summary>
        public float GetPreviewProgress(OnlineMapset mapset)
        {
            if(!IsPreviewingMapset(mapset))
                return 0f;
            return MusicController.Progress;
        }

        /// <summary>
        /// Returns the sprite name of the api provider icon.
        /// </summary>
        public string GetProviderIcon(ApiProviderType type)
        {
            return Api.GetProvider(type).IconName;
        }

        /// <summary>
        /// Sets whether the songs list is currently scrolled down.
        /// </summary>
        public void SetScrolledDown(bool isScrolledDown)
        {
            this.isScrolledDown.Value = isScrolledDown;
        }

        /// <summary>
        /// Requests for search list top scroll.
        /// </summary>
        public void RequestScrollTop()
        {
            OnScrollTopRequest?.Invoke();
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            MusicController.Stop();
            MusicController.MountAudio(null);
            MusicController.OnEnd += OnMusicEnd;

            RequestMapsets();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            MusicController.OnEnd -= OnMusicEnd;

            ResetOptions();
            ResetPreviewingMapset();
            StopMapsetRequest();
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            // Revert music back to selected map's music.
            MusicController.MountAudio(MapSelection.Music.Value);
            MusicController.Play();
        }

        /// <summary>
        /// Resets search option to initial state.
        /// </summary>
        private void ResetOptions()
        {
            Options = new SearchOptions();
            Options.ApiProvider.OnNewValue += delegate { OnSearchOptionChanged(); };
            Options.Mode.OnNewValue += delegate { OnSearchOptionChanged(); };
            Options.Category.OnNewValue += delegate { OnSearchOptionChanged(); };
            Options.Genre.OnNewValue += delegate { OnSearchOptionChanged(); };
            Options.Language.OnNewValue += delegate { OnSearchOptionChanged(); };
            Options.Sort.OnNewValue += delegate { OnSearchOptionChanged(); };
            Options.HasVideo.OnNewValue += delegate { OnSearchOptionChanged(); };
            Options.HasStoryboard.OnNewValue += delegate { OnSearchOptionChanged(); };
            Options.IsDescending.OnNewValue += delegate { OnSearchOptionChanged(); };
            Options.SearchTerm.OnNewValue += delegate { OnSearchOptionChanged(); };
        }

        /// <summary>
        /// Resets currentlying previewing mapset.
        /// </summary>
        private void ResetPreviewingMapset()
        {
            if(previewingMapset.Value == null)
                return;

            MusicController.Stop();
            MusicController.MountAudio(null);
            musicAgent.Remove();
            previewingMapset.Value = null;
        }

        /// <summary>
        /// Stops currently on-going mapset request if exists.
        /// </summary>
        private void StopMapsetRequest()
        {
            if(mapsetsRequest.Value == null)
                return;

            mapsetsRequest.Value.Dispose();
            mapsetsRequest.Value = null;
        }

        /// <summary>
        /// Event called on search option value change.
        /// </summary>
        private void OnSearchOptionChanged()
        {
            Options.Cursor = null;
            RequestMapsets();
        }

        /// <summary>
        /// Event called when music audio is loaded from cacher.
        /// </summary>
        private void OnMusicAudioLoaded(IMusicAudio audio)
        {
            MusicController.MountAudio(audio);
            MusicController.Play();
        }

        /// <summary>
        /// Event called when the mapsets search response has been returned.
        /// </summary>
        private void OnMapsetsResponse(MapsetsResponse response)
        {
            bool hadCursor = Options.HasCursor;
            if (response.IsSuccess)
            {
                mapsetList.ModifyValue(mapsets =>
                {
                    // If there was previously no cursor, this must be a fresh search using different options since the last search.
                    if (!Options.HasCursor)
                        mapsets.Clear();
                    mapsets.AddRange(response.Mapsets);
                    Options.Cursor = response.Cursor;
                });
            }
            else
            {
                mapsetList.ModifyValue(mapsets => mapsets.Clear());
            }
            OnMapsetListChange?.Invoke(mapsetList.Value, hadCursor);
            StopMapsetRequest();
        }

        /// <summary>
        /// Event called on preview music end.
        /// </summary>
        private void OnMusicEnd()
        {
            ResetPreviewingMapset();
        }
    }
}