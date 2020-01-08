using System.Collections.Generic;
using PBGame.Skins;
using PBFramework;
using PBFramework.Audio;
using UnityEngine;

namespace PBGame.Stores
{
    public interface ISkinAssetStore {

        /// <summary>
        /// Returns all the audio lookup names loaded for the current skin.
        /// </summary>
        IEnumerable<string> AudioNames { get; }

        /// <summary>
        /// Returns all the texture lookup names loaded for the current skin.
        /// </summary>
        IEnumerable<string> TextureNames { get; }


        /// <summary>
        /// Loads all assets in the skin directory.
        /// </summary>
        IPromise Load();

        /// <summary>
        /// Unloads all assets currently loaded.
        /// </summary>
        void Unload();

        /// <summary>
        /// Returns the audio associated with the specified name.
        /// </summary>
        ISkinnable<IEffectAudio> GetAudio(string name);

        /// <summary>
        /// Returns the texture associated with the specified name.
        /// </summary>
        ISkinnable<Texture2D> GetTexture(string name);
    }
}