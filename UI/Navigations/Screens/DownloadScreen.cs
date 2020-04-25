using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Download;
using PBGame.Maps;
using PBGame.Assets.Caching;
using PBGame.Graphics;
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
    public class DownloadScreen : BaseScreen, IDownloadScreen {

        private UguiSprite bgSprite;
        private SearchMenu searchMenu;
        private ResultList resultList;

        private DownloadState state;

        private CacherAgent<IMusicAudio> musicAgent;

        private bool wasMusicLooping;


        protected override int ScreenDepth => ViewDepths.DownloadScreen;

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset, IWebMusicCacher musicCacher)
        {
            Dependencies = Dependencies.Clone();
            Dependencies.Cache(state = new DownloadState());

            musicAgent = new CacherAgent<IMusicAudio>(musicCacher);
            musicAgent.OnFinished += OnMusicAudioLoaded;

            // TODO: Hook state with API search.

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

            MusicController.OnEnd += OnMusicEnded;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            musicAgent.Remove();

            state.PreviewingMapset.OnValueChanged -= OnPreviewMapsetChange;
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
    }
}