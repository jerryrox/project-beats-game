using System;
using System.Collections;
using System.Collections.Generic;
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
        private SongMenuButton backButton;
        private SongMenuButton randomButton;
        private SongMenuButton prevButton;
        private SongMenuButton nextButton;
        private SongMenuButton playButton;
        private PreviewBox previewBox;


        [InitWithDependency]
        private void Init(IScreenNavigator screenNavigator, IMapSelection mapSelection, IMapManager mapManager)
        {
            bgSprite = CreateChild<UguiSprite>("bg", -1);
            {
                bgSprite.Anchor = Anchors.Fill;
                bgSprite.RawSize = Vector2.zero;
                bgSprite.Color = new Color(0f, 0f, 0f, 0.125f);
            }
            backButton = CreateChild<SongMenuButton>("back", 0);
            {
                backButton.Anchor = Anchors.LeftStretch;
                backButton.Pivot = Pivots.Left;
                backButton.Width = 100f;
                backButton.X = 0f;
                backButton.OffsetTop = 0f;
                backButton.OffsetBottom = 0f;
                backButton.IconName = "icon-arrow-left";

                backButton.OnPointerDown += () =>
                {
                    screenNavigator.Show<HomeScreen>();
                };
            }
            randomButton = CreateChild<SongMenuButton>("random", 1);
            {
                randomButton.Anchor = Anchors.LeftStretch;
                randomButton.Pivot = Pivots.Left;
                randomButton.X = 670f;
                randomButton.Width = 80f;
                randomButton.OffsetTop = 0f;
                randomButton.OffsetBottom = 0f;
                randomButton.IconName = "icon-random";

                randomButton.OnPointerDown += () =>
                {
                    mapSelection.SelectMapset(mapManager.DisplayedMapsets.GetRandom());
                };
            }
            prevButton = CreateChild<SongMenuButton>("prev", 2);
            {
                prevButton.Anchor = Anchors.LeftStretch;
                prevButton.Pivot = Pivots.Left;
                prevButton.X = 750f;
                prevButton.Width = 80f;
                prevButton.OffsetTop = 0f;
                prevButton.OffsetBottom = 0f;
                prevButton.IconName = "icon-backward";

                prevButton.OnPointerDown += () =>
                {
                    mapSelection.SelectMapset(mapManager.DisplayedMapsets.GetPrevious(mapSelection.Mapset));
                };
            }
            nextButton = CreateChild<SongMenuButton>("next", 3);
            {
                nextButton.Anchor = Anchors.LeftStretch;
                nextButton.Pivot = Pivots.Left;
                nextButton.X = 830f;
                nextButton.Width = 80f;
                nextButton.OffsetTop = 0f;
                nextButton.OffsetBottom = 0f;
                nextButton.IconName = "icon-forward";

                nextButton.OnPointerDown += () =>
                {
                    mapSelection.SelectMapset(mapManager.DisplayedMapsets.GetNext(mapSelection.Mapset));
                };
            }
            playButton = CreateChild<SongMenuButton>("play", 4);
            {
                playButton.Anchor = Anchors.RightStretch;
                playButton.Pivot = Pivots.Right;
                playButton.X = 0f;
                playButton.Width = 100f;
                playButton.OffsetTop = 0f;
                playButton.OffsetBottom = 0f;
                playButton.IconName = "icon-play";

                playButton.OnPointerDown += () =>
                {
                    screenNavigator.Show<PrepareScreen>();
                };
            }
            previewBox = CreateChild<PreviewBox>("preview", 5);
            {
                previewBox.Anchor = Anchors.LeftStretch;
                previewBox.Pivot = Pivots.Left;
                previewBox.X = 100f;
                previewBox.Width = 560f;
                previewBox.OffsetTop = -18f;
                previewBox.OffsetBottom = 18f;

                previewBox.OnPointerDown += () =>
                {
                    screenNavigator.Show<PrepareScreen>();                 
                };
            }
        }
    }
}