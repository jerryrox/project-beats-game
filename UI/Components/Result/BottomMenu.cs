using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Result
{
    public class BottomMenu : UguiObject {

        private IconButton backButton;

        private IconButton shareButton;
        private IconButton replayButton;
        private IconButton retryButton;


        [ReceivesDependency]
        private ResultModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            var bg = CreateChild<UguiSprite>("bg");
            {
                bg.Anchor = AnchorType.Fill;
                bg.Offset = Offset.Zero;
                bg.Color = Color.black;
            }
            backButton = CreateChild<IconButton>("back");
            {
                backButton.Anchor = AnchorType.LeftStretch;
                backButton.Pivot = PivotType.Left;
                backButton.Width = 100f;
                backButton.X = 0f;
                backButton.IconName = "icon-arrow-left";

                backButton.SetOffsetVertical(0f);

                backButton.OnTriggered += Model.ToPrepare;
            }

            var rightGrid = CreateChild<UguiGrid>("right-grid");
            {
                rightGrid.Limit = 0;
                rightGrid.Anchor = AnchorType.Fill;
                rightGrid.Offset = Offset.Zero;
                rightGrid.Alignment = TextAnchor.MiddleRight;
                rightGrid.Axis = GridLayoutGroup.Axis.Horizontal;
                rightGrid.CellWidth = 100f;

                shareButton = rightGrid.CreateChild<IconButton>("share");
                {
                    shareButton.IconName = "icon-share";
                    shareButton.OnTriggered += Model.Share;
                }
                replayButton = rightGrid.CreateChild<IconButton>("replay");
                {
                    replayButton.IconName = "icon-replay";
                    replayButton.OnTriggered += Model.Replay;
                }
                retryButton = rightGrid.CreateChild<IconButton>("retry");
                {
                    retryButton.IconName = "icon-retry";
                    retryButton.OnTriggered += Model.Retry;
                }

                InvokeAfterFrames(1, () => rightGrid.CellHeight = rightGrid.Height);
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.AllowsRetry.BindAndTrigger(OnAllowsRetryChanged);
            Model.HasReplay.BindAndTrigger(OnHasReplayChanged);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.AllowsRetry.Unbind(OnAllowsRetryChanged);
            Model.HasReplay.Unbind(OnHasReplayChanged);
        }

        /// <summary>
        /// Event called when the retry allow flag has changed.
        /// </summary>
        private void OnAllowsRetryChanged(bool allowsRetry)
        {
            retryButton.Active = allowsRetry;
        }

        /// <summary>
        /// Event called when the replay existence flag has changed.
        /// </summary>
        private void OnHasReplayChanged(bool hasReplay)
        {
            replayButton.Active = hasReplay;
        }
    }
}