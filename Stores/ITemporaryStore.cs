using System.IO;

namespace PBGame.Stores
{
    public interface ITemporaryStore
    {
        /// <summary>
        /// Returns the directory where all the temporary files are to be placed in.
        /// </summary>
        DirectoryInfo BaseDirectory { get; }


        /// <summary>
        /// Returns the temporary file representing the replay data.
        /// </summary>
        FileInfo GetReplayDataFile(string id);

        /// <summary>
        /// Clears all temporary resources in the store.
        /// </summary>
        void Clear();
    }
}