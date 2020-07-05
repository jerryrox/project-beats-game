using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Stores;
using PBGame.Networking.API;
using PBGame.Networking.Maps;
using PBGame.Notifications;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

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
        private bool IsPreviewing => mapset == Model.PreviewingMapset.Value;

        [ReceivesDependency]
        private DownloadModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Color = new Color(0.125f, 0.125f, 0.125f);

            grid = CreateChild<UguiGrid>("grid", 0);
            {
                grid.Anchor = AnchorType.Fill;
                grid.Offset = Offset.Zero;

                downloadButton = grid.CreateChild<HoverableTrigger>("download", 0);
                {
                    downloadButton.CreateIconSprite(spriteName: "icon-download", size: 24f);
                    downloadButton.UseDefaultHoverAni();

                    downloadButton.IsClickToTrigger = true;

                    downloadButton.OnTriggered += () => Model.DownloadMapset(mapset);
                }
                playButton = grid.CreateChild<HoverableTrigger>("play", 1);
                {
                    playButton.CreateIconSprite(spriteName: "icon-play", size: 24f);
                    playButton.UseDefaultHoverAni();

                    playButton.IsClickToTrigger = true;

                    playButton.OnTriggered += () => Model.SetPreview(mapset);
                }
            }
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
            grid.CellSize = new Vector2(this.Width / 2f, this.Height);

            Reset();
            if(mapset == null)
                return;

            this.mapset = mapset;

            Model.PreviewingMapset.BindAndTrigger(OnPreviewMapsetChange);
        }

        /// <summary>
        /// Resets the current mapset context for actions.
        /// </summary>
        public void Reset()
        {
            if(mapset == null)
                return;

            Model.PreviewingMapset.OnNewValue -= OnPreviewMapsetChange;

            mapset = null;
        }

        /// <summary>
        /// Event called on previewing mapset change.
        /// </summary>
        private void OnPreviewMapsetChange(OnlineMapset mapset)
        {
            playButton.IconName = IsPreviewing ? "icon-stop" : "icon-play";
        }
    }
}