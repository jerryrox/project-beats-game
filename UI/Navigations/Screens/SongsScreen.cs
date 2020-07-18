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
    public class SongsScreen : BaseScreen<SongsModel> {

        private SearchMenu searchMenu;
        private SongList songList;
        private SongMenu songMenu;
        private Background background;


        protected override int ViewDepth => ViewDepths.SongsScreen;


        [InitWithDependency]
        private void Init()
        {
            background = CreateChild<Background>("background", 0);
            {
                background.Anchor = AnchorType.Fill;
                background.RawSize = Vector2.zero;
            }
            songList = CreateChild<SongList>("song-list", 1);
            {
                songList.Anchor = AnchorType.Fill;
                songList.Offset = new Offset(0f, 120f, 0f, 72f);
            }
            songMenu = CreateChild<SongMenu>("song-menu", 2);
            {
                songMenu.Anchor = AnchorType.BottomStretch;
                songMenu.Pivot = PivotType.Bottom;
                songMenu.SetOffsetHorizontal(0f);
                songMenu.Y = 0f;
                songMenu.Height = 72f;
            }
            searchMenu = CreateChild<SearchMenu>("search-menu", 3);
            {
                searchMenu.Anchor = AnchorType.TopStretch;
                searchMenu.Pivot = PivotType.Top;
                searchMenu.SetOffsetHorizontal(0f);
                searchMenu.Y = -MenuBarHeight;
                searchMenu.Height = 56;
            }
        }
    }
}