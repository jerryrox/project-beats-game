using System.IO;
using PBGame.Rulesets.Maps;

namespace PBGame.Rulesets
{
    public class GameParameter
    {
        /// <summary>
        /// The map to play.
        /// </summary>
        public IPlayableMap Map { get; set; }

        /// <summary>
        /// Whether the gameplay should be done in replay mode.
        /// </summary>
        public bool IsReplay => ReplayFile != null && ReplayFile.Exists;

        /// <summary>
        /// The reference file to the replay data, if applicable.
        /// </summary>
        public FileInfo ReplayFile { get; set; }
    }
}