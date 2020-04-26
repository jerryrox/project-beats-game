using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Download;
using PBGame.Maps;
using PBGame.Assets.Caching;
using PBGame.Graphics;
using PBGame.Rulesets;
using PBGame.Networking.API;
using PBGame.Networking.API.Responses;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Allocation.Caching;
using PBFramework.Dependencies;
using Coffee.UIExtensions;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class DownloadScreen : BaseScreen, IDownloadScreen
    {

        private UguiSprite bgSprite;
        private SearchMenu searchMenu;
        private ResultList resultList;

        private CacherAgent<IMusicAudio> musicAgent;

        private DownloadState state;


        protected override int ScreenDepth => ViewDepths.DownloadScreen;

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset, IWebMusicCacher musicCacher)
        {
            Dependencies = Dependencies.Clone();
            Dependencies.Cache(state = new DownloadState());

            musicAgent = new CacherAgent<IMusicAudio>(musicCacher);
            musicAgent.OnFinished += OnMusicAudioLoaded;

            ListenToFilterChange();

            bgSprite = CreateChild<UguiSprite>("bg", -1);
            {
                bgSprite.Anchor = Anchors.Fill;
                bgSprite.Offset = Offset.Zero;

                var gradient = bgSprite.AddEffect(new GradientEffect());
                gradient.Component.direction = UIGradient.Direction.Vertical;
                gradient.Component.color1 = colorPreset.Passive;
                gradient.Component.color2 = colorPreset.DarkBackground;
            }
            searchMenu = CreateChild<SearchMenu>("search-menu", 1);
            {
                searchMenu.Anchor = Anchors.Fill;
                searchMenu.Pivot = Pivots.Top;
                searchMenu.Offset = new Offset(0f, MenuBarHeight, 0f, 0f);
            }
            resultList = CreateChild<ResultList>("result-list", 0);
            {
                resultList.Anchor = Anchors.Fill;
                resultList.Offset = new Offset(8f, searchMenu.FoldedHeight + MenuBarHeight, 8f, 0f);
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            state.PreviewingMapset.Value = null;
            state.PreviewingMapset.BindAndTrigger(OnPreviewMapsetChange);
            state.OnNextPage += OnRequestNextPage;

            MusicController.OnEnd += OnMusicEnded;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            musicAgent.Remove();

            state.PreviewingMapset.OnValueChanged -= OnPreviewMapsetChange;
            state.OnNextPage -= OnRequestNextPage;

            state.ResetState();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            // Stop preview music in case it's currently playing.
            state.PreviewingMapset.Value = null;

            MusicController.MountAudio(MapSelection.Music);
            MusicController.Play();
        }

        /// <summary>
        /// Requests for mapset list from API.
        /// </summary>
        private void RequestMapsetList(bool isNextPage)
        {
            // Stop on-going request first.
            if (state.SearchRequest.Value != null)
            {
                state.SearchRequest.Value.Dispose();
                state.SearchRequest.Value = null;
            }

            // Verify requestable.
            var api = ApiManager.GetApi(state.ApiProvider.Value);
            if (api == null)
                return;
            var request = api.RequestFactory.GetMapsetList();
            if (request == null)
            {
                // TODO: Show notification that this request is not supported on current provider.
                return;
            }

            // Cursor should be assigned if requesting next page.
            if (isNextPage)
            {
                request.CursorName = state.CursorName;
                request.CursorValue = state.CursorValue;
                request.CursorId = state.CursorId;
            }
            else
            {
                state.CursorName = "";
                state.CursorValue = 0;
                state.CursorId = 0;
            }

            request.Mode = state.Mode.Value.GetIndex();
            request.Category = state.Category.Value;
            request.Genre = state.Genre.Value;
            request.Language = state.Language.Value;
            request.Sort = state.Sort.Value;
            request.IsDescending = state.IsDescending.Value;
            request.HasVideo = state.HasVideo.Value;
            request.HasStoryboard = state.HasStoryboard.Value;
            request.SearchTerm = state.SearchTerm.Value;
            request.OnRequestEnd += OnMapsetListResponse;

            state.SearchRequest.Value = request;
            api.Request(request);
        }

        /// <summary>
        /// Listens to change in search filters in state to make a search request when changed.
        /// </summary>
        private void ListenToFilterChange()
        {
            state.Mode.OnValueChanged += delegate { RequestMapsetList(false); };
            state.Category.OnValueChanged += delegate { RequestMapsetList(false); };
            state.Genre.OnValueChanged += delegate { RequestMapsetList(false); };
            state.Language.OnValueChanged += delegate { RequestMapsetList(false); };
            state.RankState.OnValueChanged += delegate { RequestMapsetList(false); };
            state.Sort.OnValueChanged += delegate { RequestMapsetList(false); };
            state.HasVideo.OnValueChanged += delegate { RequestMapsetList(false); };
            state.HasStoryboard.OnValueChanged += delegate { RequestMapsetList(false); };
            state.IsDescending.OnValueChanged += delegate { RequestMapsetList(false); };
            state.SearchTerm.OnValueChanged += delegate { RequestMapsetList(false); };
        }

        /// <summary>
        /// Event called on mapset list request end.
        /// </summary>
        private void OnMapsetListResponse(IMapsetListResponse response)
        {
            if (response.IsSuccess)
            {
                state.ModifyResults(resulsts =>
                {
                    resulsts.Clear();
                    resulsts.AddRange(response.Mapsets);
                });
            }
            else
            {
                state.ModifyResults(results => results.Clear());
            }
            state.SearchRequest.Value = null;
        }

        /// <summary>
        /// Event called on music controller playback finish.
        /// </summary>
        private void OnMusicEnded()
        {
            state.PreviewingMapset.Value = null;
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
        /// Event called on previewing mapset change.
        /// </summary>
        private void OnPreviewMapsetChange(OnlineMapset mapset, OnlineMapset _)
        {
            MusicController.Stop();
            MusicController.MountAudio(null);
            
            musicAgent.Remove();

            if(!string.IsNullOrEmpty(mapset?.PreviewAudio))
                musicAgent.Request(mapset.PreviewAudio);
        }

        /// <summary>
        /// Event called on request for the next page of results.
        /// </summary>
        private void OnRequestNextPage() => RequestMapsetList(true);
    }
}