using System;
using System.IO;

namespace PBGame.Skins
{
    /// <summary>
    /// Interface of an element which can be skinned by the user.
    /// </summary>
    public interface ISkinnable : IDisposable {
    
        /// <summary>
        /// Returns whether the element is a default-shipped asset.
        /// </summary>
        bool IsDefaultAsset { get; }

        /// <summary>
        /// Name of the element used for identification within the skin.
        /// </summary>
        string LookupName { get; }

        /// <summary>
        /// The original file which the element was loaded from.
        /// May be null if default asset.
        /// </summary>
        FileInfo File { get; }
    }

    public interface ISkinnable<T> : ISkinnable {
        /// <summary>
        /// The reference to the element.
        /// </summary>
        T Element { get; }
    }
}