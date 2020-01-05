using System;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;

namespace PBGame.Maps
{
    /// <summary>
    /// Provides helper functionalities wrapped over a plain list of mapsets.
    /// </summary>
    public interface IMapsetList {
    
        /// <summary>
        /// Event called when there was a change in the list.
        /// </summary>
        event Action<List<IMapset>> OnChange;


        /// <summary>
        /// Returns the mapsets list being wrapped over.
        /// </summary>
        List<IMapset> RawList { get; }

        /// <summary>
        /// Returns the number of mapsets in this list.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the mapset at specified index.
        /// </summary>
        IMapset this[int index] { get; }


        /// <summary>
        /// Adds the specified mapset to the list.
        /// </summary>
        void Add(IMapset mapset);

        /// <summary>
        /// Adds the specified range of mapsets to the list.
        /// </summary>
        void AddRange(IEnumerable<IMapset> mapsets);

        /// <summary>
        /// Clears the mapsets list.
        /// </summary>
        void Clear();

        /// <summary>
        /// Searches within the list using specified query string.
        /// </summary>
        IEnumerable<IMapset> Search(string search);

        /// <summary>
        /// Returns a random mapset within the list.
        /// </summary>
        IMapset GetRandom();

        /// <summary>
        /// Sorts the mapsets by specified sorting method.
        /// </summary>
        void Sort(MapsetSorts sort);
    }
}