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
        private MapsetSorts sortMethod = MapsetSorts.Title;

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

        public void Sort(MapsetSorts sort)
        {
            sortMethod = sort;
            switch (sort)
            {
                case MapsetSorts.Title:
                    mapsets.Sort((x, y) => x.Metadata.Title.CompareTo(y.Metadata.Title));
                    break;
                case MapsetSorts.Artist:
                    mapsets.Sort((x, y) => x.Metadata.Artist.CompareTo(y.Metadata.Artist));
                    break;
                case MapsetSorts.Creator:
                    mapsets.Sort((x, y) => x.Metadata.Creator.CompareTo(y.Metadata.Creator));
                    break;

                    // TODO: Implement
                case MapsetSorts.Date:
                    mapsets.Sort((x, y) => x.Metadata.Title.CompareTo(y.Metadata.Title));
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