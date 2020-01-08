using PBGame.Stores;
using PBFramework.Stores;

namespace PBGame.Skins
{
    public interface ISkin : IDirectoryIndex {

        /// <summary>
        /// Returns the metadata of the skin.
        /// </summary>
        SkinMetadata Metadata { get; }

        /// <summary>
        /// Returns the asset provider instance of the skin.
        /// </summary>
        ISkinAssetStore AssetStore { get; }

        /// <summary>
        /// The skin to use for fallback when an element is missing.
        /// </summary>
        ISkin Fallback { get; set; }
    }
}