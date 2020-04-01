using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MenuBar
{
    public class MusicButton : BaseMenuButton, IMusicButton {

        private bool hasOverlay = false;


        public IMusicPlaylist MusicPlaylist { get; private set; }

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            IconName = "icon-music";

            OnToggleOn += () =>
            {
                var overlay = overlayNavigator.Show<MusicMenuOverlay>();
                overlay.MusicButton = this;
                overlay.OnClose += () =>
                {
                    hasOverlay = false;
                    SetToggle(false);
                };
                hasOverlay = true;
            };

            OnToggleOff += () =>
            {
                if (hasOverlay)
                    overlayNavigator.Hide<MusicMenuOverlay>();
                hasOverlay = false;
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

        public void SetRandomMusic()
        {
            var mapset = MapManager.AllMapsets.GetRandom();
            MapSelection.SelectMapset(mapset);
            MusicPlaylist.Focus(mapset);
        }
    }
}