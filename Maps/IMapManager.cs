using System;
using System.IO;
using System.Threading.Tasks;
using PBGame.Rulesets.Maps;
using PBFramework.Threading;

namespace PBGame.Maps
{
    public interface IMapManager {

        /// <summary>
        /// Event called when a mapset has been imported.
        /// </summary>
        event Action<IMapset> OnImportMapset;

        /// <summary>
        /// Event called when a map has been deleted.
        /// </summary>
        event Action<IOriginalMap> OnDeleteMap;

        /// <summary>
        /// Event called when a mapset has been deleted.
        /// </summary>
        event Action<IMapset> OnDeleteMapset;


        /// <summary>
        /// Returns the list of all maps loaded into memory.
        /// </summary>
        IMapsetList AllMapsets { get; }

        /// <summary>
        /// Returns the list of all maps currently filtered and sorted by user preference.
        /// </summary>
        IMapsetList DisplayedMapsets { get; }

        /// <summary>
        /// Returns the search term last applied to maps.
        /// </summary>
        string LastSearch { get; }


        /// <summary>
        /// Clears loaded maps from memory and reloads from disk.
        /// </summary>
        Task Reload(TaskListener listener = null);

        /// <summary>
        /// Loads only a single mapset from disk.
        /// </summary>
        Task<IMapset> Load(Guid id, TaskListener<IMapset> listener = null);

        /// <summary>
        /// Imports the specified file as a mapset.
        /// </summary>
        Task<IMapset> Import(FileInfo file, TaskListener<IMapset> listener = null);

        /// <summary>
        /// Filters out mapsets that does not fulfil the specified filter.
        /// </summary>
        void Search(string filter);

        /// <summary>
        /// Sorts the displayed mapsets based on specified sorting method.
        /// </summary>
        void Sort(MapsetSortType sort);

        /// <summary>
        /// Deletes the specified map from the current mapset.
        /// </summary>
        void DeleteMap(IOriginalMap map);

        /// <summary>
        /// Deletes the specified mapset.
        /// </summary>
        void DeleteMapset(IMapset mapset);

        /// <summary>
        /// Returns a random mapset from displayed mapsets.
        /// </summary>
        IMapset GetRandom();
    }
}