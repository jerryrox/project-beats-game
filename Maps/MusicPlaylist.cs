using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using UnityEngine;

namespace PBGame.Maps
{
    public class MusicPlaylist : IMusicPlaylist {

        private IMapManager mapManager;
        private IMapSelection mapSelection;

        private List<IMapset> playlist = new List<IMapset>();

        private int index = 0;


        public MusicPlaylist(IMapManager mapManager, IMapSelection mapSelection)
        {
            this.mapManager = mapManager;
            this.mapSelection = mapSelection;

            mapManager.AllMapsets.OnChange += OnAllMapsetsChange;
            mapSelection.Mapset.OnNewValue += OnSelectedMapsetChange;
        }

        public void Refill()
        {
            playlist.Clear();
            if(mapManager == null) return;

            playlist.AddRange(mapManager.AllMapsets);
            RandomizePlaylist();
        }

        public void Focus(IMapset mapset)
        {
            for(int i=0; i<playlist.Count; i++)
            {
                if(playlist[i] == mapset)
                {
                    index = i;
                    break;
                }
            }
        }

        public IMapset GetNext()
        {
            if(playlist.Count == 0) return null;
            index ++;
            if(index >= playlist.Count)
                index = 0;
            return playlist[index];
        }

        public IMapset GetCurrent()
        {
            if(playlist.Count == 0) return null;
            if(index >= playlist.Count)
                index = playlist.Count - 1;
            return playlist[index];
        }

        public IMapset GetPrevious()
        {
            if(playlist.Count == 0) return null;
            index --;
            if(index < 0)
                index = playlist.Count - 1;
            return playlist[index];
        }

        /// <summary>
        /// Randomizes the playlist.
        /// </summary>
        private void RandomizePlaylist()
        {
            for (int i = 0; i < playlist.Count; i++)
            {
                var targetInx = Random.Range(0, playlist.Count);
                var backup = playlist[i];

                playlist[i] = playlist[targetInx];
                playlist[targetInx] = backup;
            }
        }

        /// <summary>
        /// Event called from map manager when the list of all mapsets have been changed.
        /// </summary>
        private void OnAllMapsetsChange(List<IMapset> mapsets)
        {
            Refill();
        }

        /// <summary>
        /// Event called when the selected mapset has changed.
        /// </summary>
        private void OnSelectedMapsetChange(IMapset mapset)
        {
            Focus(mapset);
        }
    }
}