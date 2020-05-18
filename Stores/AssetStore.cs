using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Stores
{
    /// <summary>
    /// Provides utility functiona for derived asset store types to have consistent IO logics.
    /// Or, this can be used as-is.
    /// </summary>
    public class AssetStore {

        /// <summary>
        /// Types of supported extensions for an audio asset.
        /// </summary>
        protected static readonly string[] AudioExtensions = new string[] {
            ".wav",
            ".mp3"
        };

        /// <summary>
        /// Types of supported extensions for an image asset.
        /// </summary>
        protected static readonly string[] ImageExtensions = new string[] {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp"
        };

        protected DirectoryInfo baseDirectory;


        public AssetStore(DirectoryInfo baseDirectory)
        {
            this.baseDirectory = baseDirectory;
        }

        /// <summary>
        /// Returns the audio file with specified lookup name.
        /// </summary>
        public FileInfo FindAudio(string lookupName) => FindAsset(lookupName, AudioExtensions);

        /// <summary>
        /// Returns the image file with specified lookup name.
        /// </summary>
        public FileInfo FindImage(string lookupName) => FindAsset(lookupName, ImageExtensions);

        /// <summary>
        /// Returns a file with matching lookup name and any of the extensions.
        /// </summary>
        public FileInfo FindAsset(string lookupName, string[] extensions)
        {
            foreach (var extension in extensions)
            {
                string path = Path.Combine(baseDirectory.FullName, $"{lookupName}{extension}");
                if (File.Exists(path))
                    return new FileInfo(path);
            }
            return null;
        }
    }
}