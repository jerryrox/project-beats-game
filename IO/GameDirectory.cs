using System.IO;
using UnityEngine;

namespace PBGame.IO
{
    /// <summary>
    /// Provides static constant game directories.
    /// </summary>
    public static class GameDirectory {

        /// <summary>
        /// Maps directory.
        /// </summary>
        public static readonly DirectoryInfo Maps;

        /// <summary>
        /// Configurations directory.
        /// </summary>
        public static readonly DirectoryInfo Configs;

        


        static GameDirectory()
        {
            Maps = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "maps"));
            Configs = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "configs"));

            Maps.Create();
            Configs.Create();
        }
    }
}