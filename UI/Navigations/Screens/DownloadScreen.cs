using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Download;
using PBGame.Maps;
using PBGame.Assets.Caching;
using PBGame.Graphics;
using PBGame.Rulesets;
using PBGame.Networking.API;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBGame.Networking.Maps;
using PBGame.Notifications;
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
        private UguiObject resultArea;
        private ResultList resultList;
        private ResultLoader resultLoader;

        private CacherAgent<IMusicAudio> musicAgent;

        private DownloadState state;


        protected override int ScreenDepth => ViewDepths.DownloadScreen;

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IApi Api { get; set; }

        [ReceivesDependency]
        private INotificationBox NotificationBox { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset, IWebMusicCacher musicCacher)
        {
            Dependencies = Dependencies.Clone();
            Dependencies.Cache(state = new DownloadState());

            musicAgent = new CacherAgent<IMusicAudio>(musicCacher);
            musicAgent.OnFinished += OnMusicAudioLoaded;

            bgSprite = CreateChild<UguiSprite>("bg", -1);
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.Offset = Offset.Zero;

                var gradient = bgSprite.AddEffect(new GradientEffect());
                gradient.Component.direction = UIGradient.Direction.Vertical;
                gradient.Component.color1 = colorPreset.Passive;
                gradient.Component.color2 = colorPreset.DarkBackground;
            }
            searchMenu = CreateChild<SearchMenu>("search-menu", 1);
            {
                searchMenu.Anchor = AnchorType.Fill;
                searchMenu.Pivot = PivotType.Top;
                searchMenu.Offset = new Offset(0f, MenuBarHeight, 0f, 0f);
            }
            resultArea = CreateChild<UguiObject>("result-area", 1);
            {
                resultArea.Anchor = AnchorType.Fill;
                resultArea.Offset = new Offset(0f, searchMenu.FoldedHeight + MenuBarHeight, 0f, 0f);

                resultList = resultArea.CreateChild<ResultList>("list", 0);
                {
                    resultList.Anchor = AnchorType.Fill;
                    resultList.Offset = new Offset(8f, 0f);
                }
                resultLoader = resultArea.CreateChild<ResultLoader>("loader", 1);
                {
                    resultLoader.Anchor = AnchorType.Fill;
                    resultLoader.Offset = Offset.Zero;
                }
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            state.PreviewingMapset.Value = null;
            state.PreviewingMapset.BindAndTrigger(OnPreviewMapsetChange);
            state.OnNextPage += RequestMapsetList;
            state.OnRequestList += RequestMapsetList;
            state.SearchRequest.BindAndTrigger(OnRequestChange);

            MusicController.OnEnd += OnMusicEnded;

            RequestMapsetList();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            musicAgent.Remove();

            state.PreviewingMapset.OnNewValue -= OnPreviewMapsetChange;
            state.OnNextPage -= RequestMapsetList;
            state.OnRequestList -= RequestMapsetList;
            state.SearchRequest.OnNewValue -= OnRequestChange;

            state.ResetState();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            // Stop preview music in case it's currently playing.
            state.PreviewingMapset.Value = null;

            MusicController.MountAudio(MapSelection.Music.Value);
            MusicController.Play();
        }

        /// <summary>
        /// Requests for mapset list from API.
        /// </summary>
        private void RequestMapsetList()
        {
            // Stop on-going request first.
            if (state.SearchRequest.Value != null)
            {
                state.SearchRequest.Value.Dispose();
                state.SearchRequest.Value = null;
            }

            // Verify requestable.
            var api = Api.GetProvider(state.ApiProvider.Value);
            if (api == null)
                return;

            var request = api.Mapsets();
            // Cursor should be assigned if requesting next page.
            if (state.IsRequestingNextPage)
            {
                request.Cursor = state.Cursor;
            }

            request.GameMode = (GameModeType)state.Mode.Value.GetIndex();
            request.Category = state.Category.Value;
            request.Genre = state.Genre.Value;
            request.Language = state.Language.Value;
            request.Sort = state.Sort.Value;
            request.IsDescending = state.IsDescending.Value;
            request.HasVideo = state.HasVideo.Value;
            request.HasStoryboard = state.HasStoryboard.Value;
            request.Query = state.SearchTerm.Value;

            request.Response.OnNewValue += OnMapsetListResponse;

            state.SearchRequest.Value = request;
            Api.Request(request);
        }

        /// <summary>
        /// Event called on mapset list request end.
        /// </summary>
        private void OnMapsetListResponse(MapsetsResponse response)
        {
            bool isNextPage = state.IsRequestingNextPage;

            if (response.IsSuccess)
            {
                state.Cursor = response.Cursor;
                
                state.ModifyResults(resulsts =>
                {
                    if(!isNextPage)
                        resulsts.Clear();
                    resulsts.AddRange(response.Mapsets);
                });
            }
            else
            {
                if(!isNextPage)
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
        private void OnPreviewMapsetChange(OnlineMapset mapset)
        {
            MusicController.Stop();
            MusicController.MountAudio(null);
            
            musicAgent.Remove();

            if(!string.IsNullOrEmpty(mapset?.PreviewAudio))
                musicAgent.Request(mapset.PreviewAudio);
        }

        /// <summary>
        /// Event called on mapset list request object change.
        /// </summary>
        private void OnRequestChange(MapsetsRequest request)
        {
            if(request == null)
                resultLoader.Hide();
            else
                resultLoader.Show();
        }
    }
}