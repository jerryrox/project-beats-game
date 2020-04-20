using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBFramework.UI.Navigations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public class MusicButton : BaseMenuButton {

        private bool hasOverlay = false;


        /// <summary>
        /// Returns the music playlist in use.
        /// </summary>
        public IMusicPlaylist MusicPlaylist { get; private set; }

        protected override string IconSpritename => "icon-music";

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init(IOverlayNavigator overlayNavigator)
        {
            OnFocused += (isFocused) =>
            {
                if (isFocused)
                {
                    var overlay = overlayNavigator.Show<MusicMenuOverlay>();
                    overlay.MusicButton = this;
                    overlay.OnClose += () =>
                    {
                        hasOverlay = false;
                        IsFocused = false;
                    };
                    hasOverlay = true;
                }
                else
                {
                    if (hasOverlay)
                        overlayNavigator.Hide<MusicMenuOverlay>();
                    hasOverlay = false;
                }
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