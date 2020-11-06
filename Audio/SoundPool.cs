using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Audio;

namespace PBGame.Audio
{
    public class SoundPool : ISoundPool
    {
        private List<IEffectController> pool = new List<IEffectController>();
        private List<IEffectController> persistentPool = new List<IEffectController>();

        private float baseVolume;

        private ISoundTable soundTable;
        private int poolIndex;
        private int persistentPoolIndex;


        public SoundPool(ISoundTable soundTable, int poolSize = 16, int persistentPoolSize = 4)
        {
            this.soundTable = soundTable;

            for (int i = 0; i < poolSize; i++)
                pool.Add(CreateEffectController());
            for (int i = 0; i < persistentPoolSize; i++)
                persistentPool.Add(CreateEffectController());

            SetVolume(1f);
        }

        public IEffectController Play(string lookupName, float volumeScale = 1f)
        {
            if(soundTable == null)
                return null;
            return Play(soundTable.GetAudio(lookupName), volumeScale);
        }

        public IEffectController Play(IEffectAudio effect, float volumeScale = 1f)
        {
            var controller = NextController(false);
            controller.MountAudio(effect);
            controller.Play();
            controller.SetVolume(baseVolume * volumeScale);
            return controller;
        }

        public IEnumerable<IEffectController> Play(IEnumerable<IEffectAudio> effects, float volumeScale = 1f)
        {
            foreach (var audio in effects)
            {
                var controller = NextController(false);
                controller.MountAudio(audio);
                controller.Play();
                controller.SetVolume(baseVolume * volumeScale);
                yield return controller;
            }
        }

        public IEffectController PlayPersistent(IEffectAudio effect, float volumeScale = 1f)
        {
            var controller = NextController(true);
            controller.MountAudio(effect);
            controller.Play();
            controller.SetVolume(baseVolume * volumeScale);
            return controller;
        }

        public void SetVolume(float volume)
        {
            baseVolume = volume;
            foreach (var controller in pool)
                controller.SetVolume(volume);
            foreach (var controller in persistentPool)
                controller.SetVolume(volume);
        }

        public void UnmountAll()
        {
            foreach(var controller in pool)
                controller.MountAudio(null);
            foreach (var controller in persistentPool)
                controller.MountAudio(null);
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