using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Audio;
using PBFramework.Assets.Atlasing;
using PBFramework.Debugging;

namespace PBGame.Audio
{
    public class DefaultSoundTable : ISoundTable {

        private ResourceAudioAtlas audioAtlas;
        private Dictionary<string, IEffectAudio> sounds = new Dictionary<string, IEffectAudio>();


        public DefaultSoundTable(ResourceAudioAtlas audioAtlas)
        {
            this.audioAtlas = audioAtlas;

            LoadAudio("applause");
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
            LoadAudio("type");
            LoadAudio("warning");
        }

        public IEffectAudio GetAudio(string lookupName)
        {
            if(sounds.TryGetValue(lookupName, out IEffectAudio value))
                return value;
            Logger.LogWarning($"SoundTable.GetAudio - Audio not found for name: {lookupName}");
            return null;
        }

        public bool Contains(string lookupName) => sounds.ContainsKey(lookupName);

        /// <summary>
        /// Loads the audio asset with specified name.
        /// </summary>
        private void LoadAudio(string lookupName)
        {
            var audio = audioAtlas.Get(lookupName);
            if (audio == null)
            {
                Logger.LogWarning($"SoundTable.LoadAudio - Failed to load audio for name: {lookupName}");
                return;
            }
            sounds[lookupName] = new UnityAudio(audio);
        }
    }
}