using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Download.Result
{
    public class ActionBar : UguiSprite
    {
        private IGrid grid;
        private IconButton downloadButton;
        private IconButton playButton;

        private OnlineMapset mapset;


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

                downloadButton = grid.CreateChild<IconButton>("download", 0);
                {
                    downloadButton.IconName = "icon-download";
                    
                    downloadButton.OnTriggered += () => Model.DownloadMapset(mapset);
                }
                playButton = grid.CreateChild<IconButton>("play", 1);
                {
                    playButton.IconName = "icon-play";

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
        private void OnPreviewMapsetChange(OnlineMapset newMapset)
        {
            playButton.IconName = Model.IsPreviewingMapset(this.mapset) ? "icon-stop" : "icon-play";
        }
    }
}