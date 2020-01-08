using System.IO;
using System.Threading.Tasks;

namespace PBGame.IO
{
    /// <summary>
    /// Indicates that the object can import a file.
    /// </summary>
    public interface IImportsFile {

        /// <summary>
        /// Imports the specified file and returns whether it's a success.
        /// </summary>
        Task<bool> Import(FileInfo file);
    }
}