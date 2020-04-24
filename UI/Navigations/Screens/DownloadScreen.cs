using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Download;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class DownloadScreen : BaseScreen, IDownloadScreen {

        // TODO: Include SearchMenu component.
        private ResultList resultList;

        private DownloadState state;


        protected override int ScreenDepth => ViewDepths.DownloadScreen;

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }


        [InitWithDependency]
        private void Init()
        {
            // TODO: Hook state with music controller.
            // TODO: Hook state with API search.

            Dependencies = Dependencies.Clone();
            Dependencies.Cache(state = new DownloadState());

            resultList = CreateChild<ResultList>("result-list", 1);
            {
                resultList.Anchor = Anchors.Fill;
                resultList.Offset = new Offset(8f, 484f, 8f, 0f);
            }
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            // Stop previously playing music.
            state.PreviewingMapset.Value = null;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            // Stop preview music in case it's currently playing.
            state.PreviewingMapset.Value = null;
        }
    }
}