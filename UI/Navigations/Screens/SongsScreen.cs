using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Songs;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class SongsScreen : BaseScreen<SongsModel>, ISongScreen {

        public SearchMenu SearchMenu { get; private set; }

        public SongList SongList { get; private set; }

        public SongMenu SongMenu { get; set; }

        public Background Background { get; set; }

        protected override int ViewDepth => ViewDepths.SongsScreen;


        [InitWithDependency]
        private void Init()
        {
            Background = CreateChild<Background>("background", 0);
            {
                Background.Anchor = AnchorType.Fill;
                Background.RawSize = Vector2.zero;
            }
            SongList = CreateChild<SongList>("song-list", 1);
            {
                SongList.Anchor = AnchorType.Fill;
                SongList.Offset = new Offset(0f, 120f, 0f, 72f);
            }
            SongMenu = CreateChild<SongMenu>("song-menu", 2);
            {
                SongMenu.Anchor = AnchorType.BottomStretch;
                SongMenu.Pivot = PivotType.Bottom;
                SongMenu.SetOffsetHorizontal(0f);
                SongMenu.Y = 0f;
                SongMenu.Height = 72f;
            }
            SearchMenu = CreateChild<SearchMenu>("search-menu", 3);
            {
                SearchMenu.Anchor = AnchorType.TopStretch;
                SearchMenu.Pivot = PivotType.Top;
                SearchMenu.SetOffsetHorizontal(0f);
                SearchMenu.Y = -MenuBarHeight;
                SearchMenu.Height = 56;
            }
        }
    }
}