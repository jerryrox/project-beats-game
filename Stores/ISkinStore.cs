using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.Skins;
using PBFramework.Stores;
using PBFramework.Threading;

namespace PBGame.Stores
{
    public interface ISkinStore : IDirectoryBackedStore<Skin> {
    
        /// <summary>
        /// Returns all loaded skins.
        /// </summary>
        IEnumerable<Skin> Skins { get; }


        /// <summary>
        /// Loads all skins from the storage.
        /// </summary>
        Task Reload(ISimpleProgress progress);
    }
}