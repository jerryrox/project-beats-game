using System;
using System.Collections;
using System.Collections.Generic;
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
        private HoverableTrigger backButton;
        private HoverableTrigger randomButton;
        private HoverableTrigger prevButton;
        private HoverableTrigger nextButton;
        private HoverableTrigger playButton;
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
            backButton = CreateChild<HoverableTrigger>("back", 0);
            {
                backButton.Anchor = Anchors.LeftStretch;
                backButton.Pivot = Pivots.Left;
                backButton.Width = 100f;
                backButton.X = 0f;
                backButton.OffsetTop = 0f;
                backButton.OffsetBottom = 0f;
                
                backButton.CreateIconSprite(spriteName: "icon-arrow-left");
                backButton.UseDefaultHoverAni();

                backButton.OnTriggered += () =>
                {
                    screenNavigator.Show<HomeScreen>();
                };
            }
            randomButton = CreateChild<HoverableTrigger>("random", 1);
            {
                randomButton.Anchor = Anchors.LeftStretch;
                randomButton.Pivot = Pivots.Left;
                randomButton.X = 670f;
                randomButton.Width = 80f;
                randomButton.OffsetTop = 0f;
                randomButton.OffsetBottom = 0f;

                randomButton.CreateIconSprite(spriteName: "icon-random");
                randomButton.UseDefaultHoverAni();

                randomButton.OnTriggered += () =>
                {
                    mapSelection.SelectMapset(mapManager.DisplayedMapsets.GetRandom());
                };
            }
            prevButton = CreateChild<HoverableTrigger>("prev", 2);
            {
                prevButton.Anchor = Anchors.LeftStretch;
                prevButton.Pivot = Pivots.Left;
                prevButton.X = 750f;
                prevButton.Width = 80f;
                prevButton.OffsetTop = 0f;
                prevButton.OffsetBottom = 0f;

                prevButton.CreateIconSprite(spriteName: "icon-backward");
                prevButton.UseDefaultHoverAni();

                prevButton.OnTriggered += () =>
                {
                    mapSelection.SelectMapset(mapManager.DisplayedMapsets.GetPrevious(mapSelection.Mapset));
                };
            }
            nextButton = CreateChild<HoverableTrigger>("next", 3);
            {
                nextButton.Anchor = Anchors.LeftStretch;
                nextButton.Pivot = Pivots.Left;
                nextButton.X = 830f;
                nextButton.Width = 80f;
                nextButton.OffsetTop = 0f;
                nextButton.OffsetBottom = 0f;

                nextButton.CreateIconSprite(spriteName: "icon-forward");
                nextButton.UseDefaultHoverAni();

                nextButton.OnTriggered += () =>
                {
                    mapSelection.SelectMapset(mapManager.DisplayedMapsets.GetNext(mapSelection.Mapset));
                };
            }
            playButton = CreateChild<HoverableTrigger>("play", 4);
            {
                playButton.Anchor = Anchors.RightStretch;
                playButton.Pivot = Pivots.Right;
                playButton.X = 0f;
                playButton.Width = 100f;
                playButton.OffsetTop = 0f;
                playButton.OffsetBottom = 0f;

                playButton.CreateIconSprite(spriteName: "icon-play");
                playButton.UseDefaultHoverAni();

                playButton.OnTriggered += () =>
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

                previewBox.OnTriggered += () =>
                {
                    screenNavigator.Show<PrepareScreen>();                 
                };
            }
        }
    }
}