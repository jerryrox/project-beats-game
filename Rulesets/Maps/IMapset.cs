using System.IO;
using System.Collections.Generic;
using PBFramework.Data.Queries;
using PBFramework.Stores;

namespace PBGame.Rulesets.Maps
{
    public interface IMapset : IDirectoryIndex, IQueryableData {
    
        /// <summary>
        /// Identifier of the mapset, if exists.
        /// </summary>
        int? MapsetId { get; set; }

        /// <summary>
        /// List of maps included in the set.
        /// </summary>
        List<IMap> Maps { get; set; }

        /// <summary>
        /// Returns the metadata of the mapset.
        /// </summary>
        MapMetadata Metadata { get; }

        /// <summary>
        /// Returns the storyboard file within the mapset, if exists.
        /// </summary>
        FileInfo StoryboardFile { get; }

        /// <summary>
        /// Returns the list of files contained in this mapset.
        /// </summary>
        List<FileInfo> Files { get; }


        /// <summary>
        /// Sorts the bundled maps by difficulty for specified game mode.
        /// If the given mode cannot be evaluated for, it will be pushed to the back.
        /// </summary>
        void SortMapsByMode(GameModes gameMode);
    }
}