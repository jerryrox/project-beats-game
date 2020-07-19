using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBFramework.Data.Bindables;
using UnityEngine;

namespace PBGame.Maps
{
    public class MusicPlaylist : IMusicPlaylist {

        private Bindable<IMapset> selectedMapset = new Bindable<IMapset>()
        {
            TriggerWhenDifferent = true,
        };

        private List<IMapset> playlist = new List<IMapset>();

        private int index = 0;


        /// <summary>
        /// Returns the currently selected mapset in the playlist.
        /// </summary>
        public IReadOnlyBindable<IMapset> SelectedMapset => selectedMapset;


        public void Clear()
        {
            index = -1;
            selectedMapset.Value = null;
            playlist.Clear();
        }

        public void Refill(List<IMapset> mapsets)
        {
            Clear();
            playlist.AddRange(mapsets);

            RandomizePlaylist();
        }

        public void Focus(IMapset mapset)
        {
            if(mapset == selectedMapset.Value)
                return;
                
            for(int i=0; i<playlist.Count; i++)
            {
                if(playlist[i] == mapset)
                {
                    index = i;
                    selectedMapset.Value = playlist[i];
                    break;
                }
            }
        }

        public IMapset Next()
        {
            return selectedMapset.Value = TraversePlaylist(1, ref index);
        }

        public IMapset PeekNext()
        {
            int dummyInx = index;
            return TraversePlaylist(1, ref dummyInx);
        }

        public IMapset Previous()
        {
            return selectedMapset.Value = TraversePlaylist(-1, ref index);
        }

        public IMapset PeekPrevious()
        {
            int dummyInx = index;
            return TraversePlaylist(-1, ref dummyInx);
        }

        /// <summary>
        /// Seeks through the playlist to return the mapset after specified amount of traverses.
        /// </summary>
        private IMapset TraversePlaylist(int amount, ref int index)
        {
            if(playlist.Count == 0)
                return null;

            index = Mathf.Clamp(index, 0, playlist.Count) + amount;
            while(index < 0)
                index += playlist.Count;
            while(index >= playlist.Count)
                index -= playlist.Count;
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
    }
}