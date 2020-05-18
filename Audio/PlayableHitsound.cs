using System.Linq;
using System.Collections.Generic;
using PBGame.Stores;
using PBGame.Rulesets.Maps.ControlPoints;
using PBFramework.Audio;
using UnityEngine;

namespace PBGame.Audio
{
    /// <summary>
    /// Hitsound information in its most playable form.
    /// </summary>
    public class PlayableHitsound
    {
        private ISoundTable soundTable;
        private ISoundPool soundPool;
        private List<IEffectAudio> hitsoundAudio = new List<IEffectAudio>();

        private float volume;


        public PlayableHitsound(MapAssetStore assetStore, SampleControlPoint samplePoint, List<SoundInfo> samples,
            ISoundPool soundPool)
        {
            this.soundTable = assetStore.SoundTable;
            this.soundPool = soundPool;

            hitsoundAudio.AddRange(ExtractAudio(assetStore, samples));

            volume = samplePoint.Volume;
        }

        /// <summary>
        /// Plays the hitsounds included in this object.
        /// </summary>
        public void Play()
        {
            for (int i = 0; i < hitsoundAudio.Count; i++)
                soundPool.Play(hitsoundAudio[i], volume);
            // soundPool.Play(hitsoundAudio, volume);
        }

        private IEnumerable<IEffectAudio> ExtractAudio(MapAssetStore assetStore, List<SoundInfo> samples)
        {
            foreach(var sample in samples)
            {
                var effectName = sample.LookupNames.FirstOrDefault(n => assetStore.SoundTable.Contains(n));
                if (effectName != null)
                    yield return assetStore.SoundTable.GetAudio(effectName);
            }
        }
    }
}

