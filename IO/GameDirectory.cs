using System.IO;
using PBFramework;
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

        /// <summary>
        /// User data directory.
        /// </summary>
        public static readonly DirectoryInfo Users;

        /// <summary>
        /// Play record data directory.
        /// </summary>
        public static readonly DirectoryInfo Records;

        /// <summary>
        /// Replay data directory.
        /// </summary>
        public static readonly DirectoryInfo Replays;


        static GameDirectory()
        {
            Maps = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "maps"));
            Configs = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "configs"));
            Skins = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "skins"));
            Downloads = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "downloads"));
            Users = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "users"));
            Records = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "records"));
            Replays = Records.GetSubdirectory("replays");

            Maps.Create();
            Configs.Create();
            Skins.Create();
            Downloads.Create();
            Users.Create();
            Records.Create();
            Replays.Create();
        }
    }
}