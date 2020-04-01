using PBGame.Stores;
using PBFramework.Audio;
using PBFramework.Stores;
using UnityEngine;

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


        /// <summary>
        /// Returns the audio element from the asset store.
        /// </summary>
        ISkinnable<IEffectAudio> GetAudio(string name);

        /// <summary>
        /// Returns the texture element from the asset store.
        /// </summary>
        ISkinnable<Texture2D> GetTexture(string name);
    }
}