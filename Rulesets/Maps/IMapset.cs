using System;
using System.IO;
using System.Collections.Generic;
using PBFramework.IO;
using PBFramework.Data.Queries;
using PBFramework.Stores;

namespace PBGame.Rulesets.Maps
{
    public interface IMapset : IHasFiles, IDirectoryIndex, IQueryableData {
    
        /// <summary>
        /// Identifier of the mapset, if exists.
        /// </summary>
        int? MapsetId { get; set; }

        /// <summary>
        /// Date and time of mapset import.
        /// </summary>
        DateTime ImportedDate { get; set; }

        /// <summary>
        /// List of maps included in the set.
        /// </summary>
        List<IOriginalMap> Maps { get; set; }

        /// <summary>
        /// The storyboard file within the mapset, if exists.
        /// </summary>
        FileInfo StoryboardFile { get; set; }

        /// <summary>
        /// Returns the metadata of the mapset.
        /// </summary>
        MapMetadata Metadata { get; }


        /// <summary>
        /// Sorts the bundled maps by difficulty for specified game mode.
        /// If the given mode cannot be evaluated for, it will be pushed to the back.
        /// </summary>
        void SortMapsByMode(GameModes gameMode);
    }
}