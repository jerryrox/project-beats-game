using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PBGame.Skins;
using PBFramework;
using PBFramework.Audio;
using PBFramework.Networking;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.Stores
{
    public class SkinAssetStore : ISkinAssetStore {

        /// <summary>
        /// Types of audio extensions supported for skin asset.
        /// </summary>
        private static readonly string[] AudioExtensions = new string[]
        {
            ".mp3", ".wav"
        };

        /// <summary>
        /// Types of texture extensions supported for texture asset.
        /// </summary>
        private static readonly string[] TextureExtensions = new string[]
        {
            ".jpg", ".jpeg", ".png"
        };

        /// <summary>
        /// Audio assets currently loaded.
        /// </summary>
        protected Dictionary<string, ISkinnable<IEffectAudio>> audios = new Dictionary<string, ISkinnable<IEffectAudio>>();

        /// <summary>
        /// Texture assets currently loaded.
        /// </summary>
        protected Dictionary<string, ISkinnable<Texture2D>> textures = new Dictionary<string, ISkinnable<Texture2D>>();

        /// <summary>
        /// The target skin being serviced.
        /// </summary>
        protected ISkin skin;


        public IEnumerable<string> AudioNames => audios.Keys;

        public IEnumerable<string> TextureNames => textures.Keys;


        public SkinAssetStore(ISkin skin)
        {
            if(skin == null) throw new ArgumentNullException(nameof(skin));

            this.skin = skin;
        }

        public virtual IExplicitPromise Load()
        {
            var audioRequests = FindAudioAssets().Select(RequestAudio);
            var textureRequests = FindTextureAssets().Select(RequestTexture);
            var request = new MultiPromise(audioRequests.Union(textureRequests).ToArray());
            return request;
        }

        public virtual void Unload()
        {
            foreach(var audio in audios.Values)
                audio.Dispose();
            foreach(var texture in textures.Values)
                texture.Dispose();
        }

        public ISkinnable<IEffectAudio> GetAudio(string name)
        {
            if (audios.TryGetValue(name, out ISkinnable<IEffectAudio> value))
                return value;
            if (skin.Fallback == null)
            {
                Logger.Log($"SkinAssetStore.GetAudio - No fallback element available for asset: {name}");
                return null;
            }
            return skin.Fallback.AssetStore.GetAudio(name);
        }

        public ISkinnable<Texture2D> GetTexture(string name)
        {
            if(textures.TryGetValue(name, out ISkinnable<Texture2D> value))
                return value;
            if (skin.Fallback == null)
            {
                Logger.Log($"SkinAssetStore.GetTexture - No fallback element available for asset: {name}");
                return null;
            }
            return skin.Fallback.AssetStore.GetTexture(name);
        }

        /// <summary>
        /// Finds all audio assets in the skin directory.
        /// </summary>
        protected virtual IEnumerable<FileInfo> FindAudioAssets()
        {
            return skin.Directory.GetFiles()
                .Where(f => AudioExtensions.Contains(f.Extension.ToLower()));
        }

        /// <summary>
        /// Finds all texture assets in the skin directory.
        /// </summary>
        protected virtual IEnumerable<FileInfo> FindTextureAssets()
        {
            return skin.Directory.GetFiles()
                .Where(f => TextureExtensions.Contains(f.Extension.ToLower()));
        }

        /// <summary>
        /// Returns the audio requester instance.
        /// </summary>
        private IExplicitPromise RequestAudio(FileInfo audioFile)
        {
            var request = new EffectAudioRequest(audioFile.FullName);
            request.OnFinishedResult += (result) =>
            {
                var skinnable = new SkinnableSound()
                {
                    File = audioFile,
                    Element = result,
                    LookupName = audioFile.GetNameWithoutExtension(),
                    IsDefaultAsset = false
                };
                audios.Add(skinnable.LookupName, skinnable);
            };
            return request as IExplicitPromise;
        }

        /// <summary>
        /// Returns the texture requester instance.
        /// </summary>
        private IExplicitPromise RequestTexture(FileInfo textureFile)
        {
            var request = new TextureRequest(textureFile.FullName);
            request.OnFinishedResult += (result) =>
            {
                var skinnable = new SkinnableTexture()
                {
                    File = textureFile,
                    Element = result,
                    LookupName = textureFile.GetNameWithoutExtension(),
                    IsDefaultAsset = false
                };
                textures.Add(skinnable.LookupName, skinnable);
            };
            return request as IExplicitPromise;
        }
    }
}