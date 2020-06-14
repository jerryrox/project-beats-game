using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Audio;

namespace PBGame.Audio
{
    public class MapSoundTable : ISoundTable, IDisposable {

        private ISoundTable fallback;
        private Dictionary<string, IEffectAudio> sounds = new Dictionary<string, IEffectAudio>();


        public MapSoundTable(ISoundTable fallback)
        {
            this.fallback = fallback;
        }

        /// <summary>
        /// Sets the specified effect audio to the table.
        /// </summary>
        public void SetSound(string lookupName, IEffectAudio audio) => sounds[lookupName] = audio;

        public IEffectAudio GetAudio(string lookupName)
        {
            if(sounds.TryGetValue(lookupName, out IEffectAudio value))
                return value;
            return fallback?.GetAudio(lookupName);
        }

        public bool Contains(string lookupName)
        {
            if(fallback == null)
                return sounds.ContainsKey(lookupName);
            return sounds.ContainsKey(lookupName) || fallback.Contains(lookupName);
        }

        public void Dispose()
        {
            foreach(var audio in sounds.Values)
                audio.Dispose();
        }
    }
}