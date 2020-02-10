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
    public class SongMenu : UguiObject, ISongMenu {

        private ISprite bgSprite;


        public IMenuButton BackButton { get; private set; }

        public IMenuButton RandomButton { get; private set; }

        public IMenuButton PrevButton { get; private set; }

        public IMenuButton NextButton { get; private set; }

        public IMenuButton PlayButton { get; private set; }

        public IPreviewBox PreviewBox { get; private set; }


        [InitWithDependency]
        private void Init(IScreenNavigator screenNavigator, IMapSelection mapSelection, IMapManager mapManager)
        {
            bgSprite = CreateChild<UguiSprite>("bg", -1);
            {
                bgSprite.Anchor = Anchors.Fill;
                bgSprite.RawSize = Vector2.zero;
                bgSprite.Color = new Color(0f, 0f, 0f, 0.125f);
            }
            BackButton = CreateChild<MenuButton>("back", 0);
            {
                BackButton.Anchor = Anchors.LeftStretch;
                BackButton.Pivot = Pivots.Left;
                BackButton.Width = 100f;
                BackButton.X = 0f;
                BackButton.OffsetTop = 0f;
                BackButton.OffsetBottom = 0f;
                BackButton.IconName = "icon-arrow-left";

                BackButton.OnPointerDown += () =>
                {
                    screenNavigator.Show<HomeScreen>();
                };
            }
            RandomButton = CreateChild<MenuButton>("random", 1);
            {
                RandomButton.Anchor = Anchors.LeftStretch;
                RandomButton.Pivot = Pivots.Left;
                RandomButton.X = 670f;
                RandomButton.Width = 80f;
                RandomButton.OffsetTop = 0f;
                RandomButton.OffsetBottom = 0f;
                RandomButton.IconName = "icon-random";

                RandomButton.OnPointerDown += () =>
                {
                    mapSelection.SelectMapset(mapManager.DisplayedMapsets.GetRandom());
                };
            }
            PrevButton = CreateChild<MenuButton>("prev", 2);
            {
                PrevButton.Anchor = Anchors.LeftStretch;
                PrevButton.Pivot = Pivots.Left;
                PrevButton.X = 750f;
                PrevButton.Width = 80f;
                PrevButton.OffsetTop = 0f;
                PrevButton.OffsetBottom = 0f;
                PrevButton.IconName = "icon-backward";

                PrevButton.OnPointerDown += () =>
                {
                    mapSelection.SelectMapset(mapManager.DisplayedMapsets.GetPrevious(mapSelection.Mapset));
                };
            }
            NextButton = CreateChild<MenuButton>("next", 3);
            {
                NextButton.Anchor = Anchors.LeftStretch;
                NextButton.Pivot = Pivots.Left;
                NextButton.X = 830f;
                NextButton.Width = 80f;
                NextButton.OffsetTop = 0f;
                NextButton.OffsetBottom = 0f;
                NextButton.IconName = "icon-forward";

                NextButton.OnPointerDown += () =>
                {
                    mapSelection.SelectMapset(mapManager.DisplayedMapsets.GetNext(mapSelection.Mapset));
                };
            }
            PlayButton = CreateChild<MenuButton>("play", 4);
            {
                PlayButton.Anchor = Anchors.RightStretch;
                PlayButton.Pivot = Pivots.Right;
                PlayButton.X = 0f;
                PlayButton.Width = 100f;
                PlayButton.OffsetTop = 0f;
                PlayButton.OffsetBottom = 0f;
                PlayButton.IconName = "icon-play";

                PlayButton.OnPointerDown += () =>
                {
                    // TODO: Show PrepareScreen
                };
            }
            PreviewBox = CreateChild<PreviewBox>("preview", 5);
            {
                PreviewBox.Anchor = Anchors.LeftStretch;
                PreviewBox.Pivot = Pivots.Left;
                PreviewBox.X = 100f;
                PreviewBox.Width = 560f;
                PreviewBox.OffsetTop = -18f;
                PreviewBox.OffsetBottom = 18f;

                PreviewBox.OnPointerDown += () =>
                {
                    // TODO: Show PrepareScreen                    
                };
            }
        }
    }
}