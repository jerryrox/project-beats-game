using System;
using System.Collections;
using System.Collections.Generic;
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

        private HoverableTrigger backButton;

        private HoverableTrigger shareButton;
        private HoverableTrigger replayButton;
        private HoverableTrigger retryButton;


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
            backButton = CreateChild<HoverableTrigger>("back");
            {
                backButton.Anchor = AnchorType.LeftStretch;
                backButton.Pivot = PivotType.Left;
                backButton.Width = 100f;
                backButton.X = 0f;
                
                backButton.SetOffsetVertical(0f);
                backButton.CreateIconSprite(spriteName: "icon-arrow-left");
                backButton.UseDefaultHoverAni();

                backButton.OnTriggered += Model.ToPrepare;
            }
            retryButton = CreateChild<HoverableTrigger>("retry");
            {
                retryButton.Anchor = AnchorType.RightStretch;
                retryButton.Pivot = PivotType.Right;
                retryButton.Width = 100f;
                retryButton.X = 0f;
                
                retryButton.SetOffsetVertical(0f);
                retryButton.CreateIconSprite(spriteName: "icon-retry");
                retryButton.UseDefaultHoverAni();

                retryButton.OnTriggered += Model.Retry;
            }
            replayButton = CreateChild<HoverableTrigger>("replay");
            {
                replayButton.Anchor = AnchorType.RightStretch;
                replayButton.Pivot = PivotType.Right;
                replayButton.Width = 100f;
                replayButton.X = 100f;
                
                replayButton.SetOffsetVertical(0f);
                replayButton.CreateIconSprite(spriteName: "icon-replay");
                replayButton.UseDefaultHoverAni();

                replayButton.OnTriggered += Model.Replay;
            }
            shareButton = CreateChild<HoverableTrigger>("share");
            {
                shareButton.Anchor = AnchorType.RightStretch;
                shareButton.Pivot = PivotType.Right;
                shareButton.Width = 100f;
                shareButton.X = 200f;
                
                shareButton.SetOffsetVertical(0f);
                shareButton.CreateIconSprite(spriteName: "icon-share");
                shareButton.UseDefaultHoverAni();

                shareButton.OnTriggered += Model.Share;
            }
        }
    }
}