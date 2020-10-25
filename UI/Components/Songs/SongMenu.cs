using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.UI.Navigations.Screens;
using PBGame.Maps;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class SongMenu : UguiObject {

        private ISprite bgSprite;
        private IconButton backButton;
        private IconButton randomButton;
        private IconButton prevButton;
        private IconButton nextButton;
        private IconButton playButton;
        private PreviewBox previewBox;

        [ReceivesDependency]
        private SongsModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            bgSprite = CreateChild<UguiSprite>("bg", -1);
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.RawSize = Vector2.zero;
                bgSprite.Color = new Color(0f, 0f, 0f, 0.125f);
            }
            backButton = CreateChild<IconButton>("back", 0);
            {
                backButton.Anchor = AnchorType.LeftStretch;
                backButton.Pivot = PivotType.Left;
                backButton.Width = 100f;
                backButton.X = 0f;
                backButton.IconName = "icon-arrow-left";
                
                backButton.SetOffsetVertical(0f);

                backButton.OnTriggered += Model.NavigateToHome;
            }
            randomButton = CreateChild<IconButton>("random", 1);
            {
                randomButton.Anchor = AnchorType.LeftStretch;
                randomButton.Pivot = PivotType.Left;
                randomButton.X = 670f;
                randomButton.Width = 80f;
                randomButton.SetOffsetVertical(0f);
                randomButton.IconName = "icon-random";

                randomButton.OnTriggered += Model.SelectRandomMapset;
            }
            prevButton = CreateChild<IconButton>("prev", 2);
            {
                prevButton.Anchor = AnchorType.LeftStretch;
                prevButton.Pivot = PivotType.Left;
                prevButton.X = 750f;
                prevButton.Width = 80f;
                prevButton.SetOffsetVertical(0f);
                prevButton.IconName = "icon-backward";

                prevButton.OnTriggered += Model.SelectPrevMapset;
            }
            nextButton = CreateChild<IconButton>("next", 3);
            {
                nextButton.Anchor = AnchorType.LeftStretch;
                nextButton.Pivot = PivotType.Left;
                nextButton.X = 830f;
                nextButton.Width = 80f;
                nextButton.SetOffsetVertical(0f);
                nextButton.IconName = "icon-forward";

                nextButton.OnTriggered += Model.SelectNextMapset;
            }
            playButton = CreateChild<IconButton>("play", 4);
            {
                playButton.Anchor = AnchorType.RightStretch;
                playButton.Pivot = PivotType.Right;
                playButton.X = 0f;
                playButton.Width = 100f;
                playButton.SetOffsetVertical(0f);
                playButton.IconName = "icon-play";

                playButton.OnTriggered += Model.NavigateToPrepare;
            }
            previewBox = CreateChild<PreviewBox>("preview", 5);
            {
                previewBox.Anchor = AnchorType.LeftStretch;
                previewBox.Pivot = PivotType.Left;
                previewBox.X = 100f;
                previewBox.Width = 560f;
                previewBox.SetOffsetVertical(-18f, 18f);

                previewBox.OnTriggered += Model.NavigateToPrepare;
            }
        }
    }
}