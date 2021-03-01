using System.IO;
using PBGame.IO;

namespace PBGame.Stores
{
    public class TemporaryStore : ITemporaryStore {

        public DirectoryInfo BaseDirectory => GameDirectory.Temporary;


        public FileInfo GetReplayDataFile(string name)
        {
            return new FileInfo(Path.Combine(BaseDirectory.FullName, $"{name}.replay"));
        }

        public void Clear()
        {
            BaseDirectory.Delete(true);
            BaseDirectory.Create();
        }
    }
}