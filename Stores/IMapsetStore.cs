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
        /// Loads all mapsets from the storage.
        /// </summary>
        Task Reload(ISimpleProgress progress);
    }
}