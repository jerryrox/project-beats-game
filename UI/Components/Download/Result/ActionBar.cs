using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.API;
using PBGame.Networking.Maps;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Result
{
    public class ActionBar : UguiSprite
    {
        private IGrid grid;
        private HoverableTrigger downloadButton;
        private HoverableTrigger playButton;

        private OnlineMapset mapset;


        /// <summary>
        /// Returns whether the previewing mapset is equal to mapset being represented.
        /// </summary>
        private bool IsPreviewing => mapset == State.PreviewingMapset.Value;

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private DownloadState State { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Color = new Color(0.125f, 0.125f, 0.125f);

            grid = CreateChild<UguiGrid>("grid", 0);
            {
                grid.Anchor = Anchors.Fill;
                grid.Offset = Offset.Zero;

                downloadButton = grid.CreateChild<HoverableTrigger>();
                {
                    downloadButton.CreateIconSprite(spriteName: "icon-download", size: 24f);
                    downloadButton.UseDefaultHoverAni();

                    downloadButton.OnTriggered += OnDownloadButton;
                }
                playButton = grid.CreateChild<HoverableTrigger>();
                {
                    playButton.CreateIconSprite(spriteName: "icon-play", size: 24f);
                    playButton.UseDefaultHoverAni();

                    playButton.OnTriggered += OnPlayButton;
                }
            }

            InvokeAfterFrames(1, () =>
            {
                grid.CellSize = new Vector2(this.Width / 2f, this.Height);
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Reset();
        }

        /// <summary>
        /// Initializes the action bar for specified mapset.
        /// </summary>
        public void Setup(OnlineMapset mapset)
        {
            Reset();
            if(mapset == null)
                return;

            this.mapset = mapset;

            State.PreviewingMapset.BindAndTrigger(OnPreviewMapsetChange);
        }

        /// <summary>
        /// Resets the current mapset context for actions.
        /// </summary>
        public void Reset()
        {
            if(mapset == null)
                return;

            State.PreviewingMapset.OnValueChanged -= OnPreviewMapsetChange;

            mapset = null;
        }

        /// <summary>
        /// Event called on download button trigger.
        /// </summary>
        private void OnDownloadButton()
        {
            if(mapset == null)
                return;

            // TODO: Request download.
        }

        /// <summary>
        /// Event called on play button trigger.
        /// </summary>
        private void OnPlayButton()
        {
            if(mapset == null)
                return;

            State.PreviewingMapset.Value = IsPreviewing ? null : mapset;
        }

        /// <summary>
        /// Sets previewing state on play button icon to display whether it's currently previewing.
        /// </summary>
        private void SetPreviewing(bool isPlaying)
        {
            playButton.IconName = isPlaying ? "icon-stop" : "icon-play";
        }

        /// <summary>
        /// Event called on previewing mapset change.
        /// </summary>
        private void OnPreviewMapsetChange(OnlineMapset mapset, OnlineMapset _)
        {
            SetPreviewing(IsPreviewing);
        }
    }
}