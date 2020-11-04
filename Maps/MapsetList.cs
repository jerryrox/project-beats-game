using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;

using Random = UnityEngine.Random;

namespace PBGame.Maps
{
    public class MapsetList : IMapsetList {

        public event Action<List<IMapset>> OnChange;


        /// <summary>
        /// Raw list of mapsets.
        /// </summary>
        private List<IMapset> mapsets = new List<IMapset>();

        /// <summary>
        /// Filtering handler for user search.
        /// </summary>
        private MapQueryer mapQueryer = new MapQueryer();

        /// <summary>
        /// The last sorting method used.
        /// </summary>
        private MapsetSortType sortMethod = MapsetSortType.Title;

        /// <summary>
        /// Whether sorting should be applied implicitly on modifying the list.
        /// </summary>
        private bool implicitSort = false;


        public List<IMapset> RawList => mapsets;

        public int Count => mapsets.Count;

        public IMapset this[int index] => mapsets[index];


        public MapsetList(bool implicitSort)
        {
            this.implicitSort = implicitSort;
        }

        public void Add(IMapset mapset)
        {
            mapsets.Add(mapset);
            if(implicitSort)
                Sort(sortMethod);
            else
                InvokeChange();
        }

        public void AddRange(IEnumerable<IMapset> mapsets)
        {
            this.mapsets.AddRange(mapsets);
            if(implicitSort)
                Sort(sortMethod);
            else
                InvokeChange();
        }

        public void AddOrReplace(IMapset mapset)
        {
            // Try replace first
            for (int i = 0; i < mapsets.Count; i++)
            {
                if (mapsets[i].Id == mapset.Id)
                {
                    mapsets[i] = mapset;
                    return;
                }
            }

            // Else, add
            Add(mapset);
        }

        public void Remove(IMapset mapset)
        {
            mapsets.Remove(mapset);
            InvokeChange();
        }

        public void Clear()
        {
            mapsets.Clear();
            InvokeChange();
        }

        public IEnumerable<IMapset> Search(string search) => mapQueryer.Query(mapsets, search);

        public IMapset GetRandom()
        {
            if(mapsets.Count == 0) return null;
            return mapsets[Random.Range(0, mapsets.Count)];
        }

        public IMapset GetPrevious(IMapset mapset)
        {
            if(mapsets.Count == 0) return null;
            if(mapset == null) return null;

            var index = mapsets.IndexOf(mapset);
            if(index < 0)
                return mapsets[0];
            return mapsets[index <= 0 ? mapsets.Count - 1 : index - 1];
        }

        public IMapset GetNext(IMapset mapset)
        {
            if (mapsets.Count == 0) return null;
            if (mapset == null) return null;
            
            var index = mapsets.IndexOf(mapset);
            if (index < 0)
                return mapsets[0];
            return mapsets[index >= mapsets.Count-1 ? 0 : index + 1];
        }

        public void Sort(MapsetSortType sort)
        {
            sortMethod = sort;
            switch (sort)
            {
                case MapsetSortType.Title:
                    mapsets.Sort((x, y) => x.Metadata.Title.CompareTo(y.Metadata.Title));
                    break;
                case MapsetSortType.Artist:
                    mapsets.Sort((x, y) => x.Metadata.Artist.CompareTo(y.Metadata.Artist));
                    break;
                case MapsetSortType.Creator:
                    mapsets.Sort((x, y) => x.Metadata.Creator.CompareTo(y.Metadata.Creator));
                    break;
                case MapsetSortType.Date:
                    mapsets.Sort((x, y) => x.ImportedDate.CompareTo(y.ImportedDate));
                    break;
            }
            InvokeChange();
        }

        public IEnumerator<IMapset> GetEnumerator() => mapsets.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Invokes OnChange event.
        /// </summary>
        private void InvokeChange() => OnChange?.Invoke(mapsets);
    }
}