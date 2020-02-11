using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MenuBar
{
    public class MusicButton : IconMenuButton, IMusicButton {

        protected override string IconName => "icon-music";


        public IMusicPlaylist MusicPlaylist { get; private set; }

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnToggleOn += () =>
            {
                // TODO: Show music overlay. (Pass this interface for next/prev music control.
                // TODO: Toggle off when music overlay is hidden.
            };

            OnToggleOff += () =>
            {
                // TODO: Hide music overlay.
            };

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            MusicPlaylist = new MusicPlaylist(MapManager);
            MusicPlaylist.Refill();
            MusicPlaylist.Focus(MapSelection.Mapset);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MusicPlaylist = null;
        }

        public void SetNextMusic()
        {
            MapSelection.SelectMapset(MusicPlaylist.GetNext());
        }

        public void SetPrevMusic()
        {
            MapSelection.SelectMapset(MusicPlaylist.GetPrevious());
        }
    }
}