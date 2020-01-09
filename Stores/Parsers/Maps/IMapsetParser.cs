using System.IO;
using PBGame.Rulesets.Maps;

namespace PBGame.Stores.Parsers.Maps
{
    /// <summary>
    /// Parses the mapset data from the mapset store.
    /// </summary>
    public interface IMapsetParser {

        /// <summary>
        /// Parses and returns the mapset from specified directory.
        /// </summary>
        Mapset Parse(DirectoryInfo directory, Mapset mapset);
    }
}