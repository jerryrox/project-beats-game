using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data;
using PBFramework.Audio;

namespace PBGame.Audio
{
    public class SoundPool : ISoundPool
    {

        private List<IEffectController> pool = new List<IEffectController>();
        private List<IEffectController> persistentPool = new List<IEffectController>();

        private ISoundTable soundTable;
        private int poolIndex;
        private int persistentPoolIndex;


        public SoundPool(ISoundTable soundTable, int poolSize = 12, int persistentPoolSize = 8)
        {
            this.soundTable = soundTable;

            for (int i = 0; i < poolSize; i++)
                pool.Add(CreateEffectController());
            for (int i = 0; i < persistentPoolSize; i++)
                persistentPool.Add(CreateEffectController());
        }

        public IEffectController Play(string lookupName)
        {
            if(soundTable == null)
                return null;
            return Play(soundTable.GetAudio(lookupName));
        }

        public IEffectController Play(IEffectAudio effect)
        {
            var controller = NextController(false);
            controller.MountAudio(effect);
            controller.Play();
            return controller;
        }

        public IEnumerable<IEffectController> Play(IEnumerable<IEffectAudio> effects)
        {
            foreach (var audio in effects)
            {
                var controller = NextController(false);
                controller.MountAudio(audio);
                controller.Play();
                yield return controller;
            }
        }

        public IEffectController PlayPersistent(IEffectAudio effect)
        {
            var controller = NextController(true);
            controller.MountAudio(effect);
            controller.Play();
            return controller;
        }

        public void SetVolume(float volume)
        {
            foreach(var controller in pool)
                controller.SetVolume(volume);
            foreach (var controller in persistentPool)
                controller.SetVolume(volume);
        }

        /// <summary>
        /// Returns the effect controller.
        /// </summary>
        private IEffectController NextController(bool isPersistent)
        {
            IEffectController controller;
            if (isPersistent)
            {
                controller = persistentPool[persistentPoolIndex++];
                if(persistentPoolIndex >= persistentPool.Count)
                    persistentPoolIndex = 0;
            }
            else
            {
                controller = pool[poolIndex++];
                if (poolIndex >= pool.Count)
                    poolIndex = 0;
            }
            controller.Stop();
            return controller;
        }

        /// <summary>
        /// Creates a new effect controller instance.
        /// </summary>
        private IEffectController CreateEffectController() => EffectController.Create();
    }
}