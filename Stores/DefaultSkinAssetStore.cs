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

        public override IPromise Load()
        {
            // Default assets are only loaded once.
            if(isLoaded) return null;
            isLoaded = true;

            LoadTexture("circle-base");
            LoadTexture("circle-body");
            LoadTexture("circle-cover");
            LoadTexture("circle-glow");
            LoadTexture("circle-tick");
            LoadTexture("count1");
            LoadTexture("count2");
            LoadTexture("count3");
            LoadTexture("go");
            LoadTexture("hit-bad");
            LoadTexture("hit-good");
            LoadTexture("hit-great");
            LoadTexture("hit-miss");
            LoadTexture("hit-ok");
            LoadTexture("hit-perfect");
            LoadTexture("pause-overlay");
            LoadTexture("play-warningarrow");
            LoadTexture("ranking-A");
            LoadTexture("ranking-A-small");
            LoadTexture("ranking-B");
            LoadTexture("ranking-B-small");
            LoadTexture("ranking-C");
            LoadTexture("ranking-C-small");
            LoadTexture("ranking-D");
            LoadTexture("ranking-D-small");
            LoadTexture("ranking-S");
            LoadTexture("ranking-S-small");
            LoadTexture("ranking-SH");
            LoadTexture("ranking-SH-small");
            LoadTexture("ranking-X");
            LoadTexture("ranking-X-small");
            LoadTexture("ranking-XH");
            LoadTexture("ranking-XH-small");

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
            LoadAudio("lewel-up");
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