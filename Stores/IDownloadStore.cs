using PBFramework.Storages;

namespace PBGame.Stores
{
    public interface IDownloadStore {
    
        /// <summary>
        /// Returns the storage which holds downloaded maps.
        /// </summary>
        IFileStorage MapStorage { get; }
    }
}