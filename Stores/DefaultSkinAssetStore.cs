using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBGame.Skins;
using PBFramework;
using PBFramework.Audio;

namespace PBGame.Stores
{
    public class DefaultSkinAssetStore : SkinAssetStore {

        private bool isLoaded = false;


        public DefaultSkinAssetStore(ISkin skin) : base(skin)
        {
        }

        public override IExplicitPromise Load()
        {
            // Default assets are only loaded once.
            if(isLoaded) return null;
            isLoaded = true;

            LoadAudio("applause");
            LoadAudio("combobreak");
            LoadAudio("count1");
            LoadAudio("count2");
            LoadAudio("count3");
            LoadAudio("drum-hitclap");
            LoadAudio("drum-hitfinish");
            LoadAudio("drum-hitnormal");
            LoadAudio("drum-hitwhistle");
            LoadAudio("drum-sliderslide");
            LoadAudio("drum-slidertick");
            LoadAudio("drum-sliderwhistle");
            LoadAudio("exp-up");
            LoadAudio("failsound");
            LoadAudio("go");
            LoadAudio("heartbeat");
            LoadAudio("level-up");
            LoadAudio("menuback");
            LoadAudio("menuclick");
            LoadAudio("menuhit");
            LoadAudio("normal-hitclap");
            LoadAudio("normal-hitfinish");
            LoadAudio("normal-hitnormal");
            LoadAudio("normal-hitwhistle");
            LoadAudio("normal-sliderslide");
            LoadAudio("normal-slidertick");
            LoadAudio("normal-sliderwhistle");
            LoadAudio("notification");
            LoadAudio("sectionfail");
            LoadAudio("sectionpass");
            LoadAudio("soft-hitclap");
            LoadAudio("soft-hitfinish");
            LoadAudio("soft-hitnormal");
            LoadAudio("soft-hitwhistle");
            LoadAudio("soft-sliderslide");
            LoadAudio("soft-slidertick");
            LoadAudio("soft-sliderwhistle");
            LoadAudio("toggle-off");
            LoadAudio("toggle-on");
            LoadAudio("type");
            LoadAudio("warning");

            return null;
        }

        public override void Unload()
        {
            // Default assets will never be unloaded from the game.
        }

        /// <summary>
        /// Loads the audio asset with specified name.
        /// </summary>
        private void LoadAudio(string name)
        {
            var audio = Resources.Load(GetPath(name), typeof(AudioClip)) as AudioClip;
            if(audio == null) return;

            audios[name] = new SkinnableSound()
            {
                File = null,
                Element = new UnityAudio(audio),
                LookupName = name,
                IsDefaultAsset = true
            };
        }

        /// <summary>
        /// Returns the texture asset with specified name.
        /// </summary>
        private void LoadTexture(string name)
        {
            var texture = Resources.Load(GetPath(name), typeof(Texture2D)) as Texture2D;
            if(texture == null) return;

            textures[name] = new SkinnableTexture()
            {
                File = null,
                Element = texture,
                LookupName = name,
                IsDefaultAsset = true
            };
        }

        /// <summary>
        /// Returns the resource path for the specified name.
        /// </summary>
        private string GetPath(string name) => $"DefaultSkin/{name}";
    }
}