using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBFramework.Stores;
using PBFramework.Threading;

namespace PBGame.Stores
{
    public interface IMapsetStore : IDirectoryBackedStore<Mapset> {
    
        /// <summary>
        /// Returns all loaded mapsets.
        /// </summary>
        IEnumerable<Mapset> Mapsets { get; }


        /// <summary>
        /// Loads the mapset of specified id.
        /// </summary>
        Mapset Load(Guid id);

        /// <summary>
        /// Deletes the specified map in this mapset.
        /// </summary>
        void DeleteMap(IOriginalMap map);
    }
}