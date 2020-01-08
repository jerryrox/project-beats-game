using System.IO;
using PBGame.Skins;

namespace PBGame.Stores.Parsers.Skins
{
    /// <summary>
    /// Parses the skin data from the skin store.
    /// </summary>
    public interface ISkinParser {

        /// <summary>
        /// Parses and returns the skin from specified directory.
        /// </summary>
        Skin Parse(DirectoryInfo directory);
    }
}