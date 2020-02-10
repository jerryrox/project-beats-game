using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Songs;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class SongsScreen : BaseScreen, ISongScreen {

        public ISearchMenu SearchMenu { get; private set; }

        public ISongList SongList { get; private set; }

        public ISongMenu SongMenu { get; set; }

        public IBackground Background { get; set; }

        protected override int ScreenDepth => ViewDepths.SongsScreen;


        [InitWithDependency]
        private void Init()
        {
            Background = CreateChild<Background>("background", 0);
            {
                Background.Anchor = Anchors.Fill;
                Background.RawSize = Vector2.zero;
            }
            SongList = CreateChild<SongList>("song-list", 1);
            {
                SongList.Anchor = Anchors.Fill;
                SongList.Y = 120f;
                SongList.OffsetBottom = 72f;
                SongList.OffsetLeft = 0f;
                SongList.OffsetRight = 0f;
            }
            SongMenu = CreateChild<SongMenu>("song-menu", 2);
            {
                SongMenu.Anchor = Anchors.BottomStretch;
                SongMenu.Pivot = Pivots.Bottom;
                SongMenu.OffsetLeft = 0f;
                SongMenu.OffsetRight = 0f;
                SongMenu.Y = 0f;
                SongMenu.Height = 72f;
            }
            SearchMenu = CreateChild<SearchMenu>("search-menu", 3);
            {
                SearchMenu.Anchor = Anchors.TopStretch;
                SearchMenu.Pivot = Pivots.Top;
                SearchMenu.OffsetLeft = 0f;
                SearchMenu.OffsetRight = 0f;
                SearchMenu.Y = -64f;
                SearchMenu.Height = 56;
            }
        }
    }
}