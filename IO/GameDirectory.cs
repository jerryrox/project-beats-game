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

        /// <summary>
        /// Skins directory.
        /// </summary>
        public static readonly DirectoryInfo Skins;

        /// <summary>
        /// Downloads directory.
        /// </summary>
        public static readonly DirectoryInfo Downloads;


        static GameDirectory()
        {
            Maps = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "maps"));
            Configs = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "configs"));
            Skins = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "skins"));
            Downloads = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "downloads"));

            Maps.Create();
            Configs.Create();
            Skins.Create();
            Downloads.Create();
        }
    }
}