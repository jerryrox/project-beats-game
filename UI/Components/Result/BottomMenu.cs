using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

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
            retryButton = CreateChild<IconButton>("retry");
            {
                retryButton.Anchor = AnchorType.RightStretch;
                retryButton.Pivot = PivotType.Right;
                retryButton.Width = 100f;
                retryButton.X = 0f;
                retryButton.IconName = "icon-retry";
                
                retryButton.SetOffsetVertical(0f);

                retryButton.OnTriggered += Model.Retry;
            }
            replayButton = CreateChild<IconButton>("replay");
            {
                replayButton.Anchor = AnchorType.RightStretch;
                replayButton.Pivot = PivotType.Right;
                replayButton.Width = 100f;
                replayButton.X = 100f;
                replayButton.IconName = "icon-replay";
                
                replayButton.SetOffsetVertical(0f);

                replayButton.OnTriggered += Model.Replay;
            }
            shareButton = CreateChild<IconButton>("share");
            {
                shareButton.Anchor = AnchorType.RightStretch;
                shareButton.Pivot = PivotType.Right;
                shareButton.Width = 100f;
                shareButton.X = 200f;
                shareButton.IconName = "icon-share";
                
                shareButton.SetOffsetVertical(0f);

                shareButton.OnTriggered += Model.Share;
            }
        }
    }
}