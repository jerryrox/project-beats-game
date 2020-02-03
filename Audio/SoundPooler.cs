using System.Linq;
using System.Collections.Generic;
using PBGame.Skins;
using PBFramework.Debugging;

namespace PBGame.Audio
{
    public class SoundPooler : ISoundPooler {

        private Dictionary<string, ISoundControlPool> pools = new Dictionary<string, ISoundControlPool>(32);


        public SoundPooler(ISkin defaultSkin)
        {
            if (defaultSkin == null)
            {
                Logger.LogWarning($"SoundPooler - defaultSkin not defined. Consistency check will be skipped but it is recommended to do so.");
            }

            // Create pools and pre-define their control pool size.
            CreatePool("applause", 1);
            CreatePool("combobreak", 1);
            CreatePool("count1", 1);
            CreatePool("count2", 1);
            CreatePool("count3", 1);
            CreatePool("drum-hitclap", 2);
            CreatePool("drum-hitfinish", 2);
            CreatePool("drum-hitnormal", 2);
            CreatePool("drum-hitwhistle", 2);
            CreatePool("drum-sliderslide", 2);
            CreatePool("drum-slidertick", 2);
            CreatePool("drum-sliderwhistle", 2);
            CreatePool("exp-up", 1);
            CreatePool("failsound", 1);
            CreatePool("go", 1);
            CreatePool("heartbeat", 2);
            CreatePool("level-up", 1);
            CreatePool("menuback", 1);
            CreatePool("menuclick", 1);
            CreatePool("menuhit", 1);
            CreatePool("normal-hitclap", 2);
            CreatePool("normal-hitfinish", 2);
            CreatePool("normal-hitnormal", 2);
            CreatePool("normal-hitwhistle", 2);
            CreatePool("normal-sliderslide", 2);
            CreatePool("normal-slidertick", 2);
            CreatePool("normal-sliderwhistle", 2);
            CreatePool("notification", 1);
            CreatePool("sectionfail", 1);
            CreatePool("sectionpass", 1);
            CreatePool("soft-hitclap", 2);
            CreatePool("soft-hitfinish", 2);
            CreatePool("soft-hitnormal", 2);
            CreatePool("soft-hitwhistle", 2);
            CreatePool("soft-sliderslide", 2);
            CreatePool("soft-slidertick", 2);
            CreatePool("soft-sliderwhistle", 2);
            CreatePool("toggle-off", 1);
            CreatePool("toggle-on", 1);
            CreatePool("type", 1);
            CreatePool("warning", 1);

            // Compare with the default skin's audio lookup names for missing entry.
            if (defaultSkin != null)
            {
                foreach (var name in defaultSkin.AssetStore.AudioNames.Where(n => !pools.ContainsKey(n)))
                {
                    Logger.LogWarning($"SoundPooler - Missing pool info for sound name ({name}).");
                }
            }
        }

        public void Prepare(ISkin skin)
        {
            var assetStore = skin.AssetStore;
            foreach (var pair in pools)
            {
                var audio = assetStore.GetAudio(pair.Key);
                pair.Value.SetAudio(audio.Element);
            }
        }

        public void Play(string lookupName, float volumeScale = 1f)
        {
            // Find the pool and play the audio.
            if (pools.TryGetValue(lookupName, out ISoundControlPool value))
                value.Play(volumeScale);
            else
                Logger.LogWarning($"SoundPooler.Play - Missing lookupName: {lookupName}");
        }

        public void Stop(string lookupName)
        {
            // Find the pool and stop.
            if (pools.TryGetValue(lookupName, out ISoundControlPool value))
                value.Stop();
            else
                Logger.LogWarning($"SoundPooler.Stop - Missing lookupName: {lookupName}");
        }

        public void SetVolume(float volume)
        {
            foreach(var pool in pools.Values)
                pool.SetVolume(volume);
        }

        private void CreatePool(string name, int poolSize) => pools[name] = new SoundControlPool(poolSize);
    }
}