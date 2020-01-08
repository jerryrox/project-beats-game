using System;
using PBFramework.Audio;

namespace PBGame.Audio
{
    public class SoundControlPool : ISoundControlPool {

        /// <summary>
        /// Pool of controllers dedicated for a single effect audio.
        /// </summary>
        private IEffectController[] effectControllers;

        /// <summary>
        /// The actual audio to be played on the controllers.
        /// </summary>
        private IEffectAudio effectAudio;

        /// <summary>
        /// Index of the next controller to play the sound from.
        /// </summary>
        private int nextIndex = 0;

        /// <summary>
        /// Overall volume of the controller.
        /// </summary>
        private float volume;


        public SoundControlPool(int poolSize)
        {
            if(poolSize == 0) throw new ArgumentException("poolSize must be greater than 1.");

            effectControllers = new IEffectController[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                effectControllers[i] = EffectController.Create();
            }
        }

        public void SetAudio(IEffectAudio audio)
        {
            effectAudio = audio;
            if (audio != null)
            {
                for(int i=0; i<effectControllers.Length; i++)
                    effectControllers[i].MountAudio(audio);
            }
        }

        public void SetVolume(float volume)
        {
            this.volume = volume;
            for (int i = 0; i < effectControllers.Length; i++)
                effectControllers[i].SetVolume(volume);
        }

        public void Play(float volumeScale = 1f)
        {
            // Play
            var controller = effectControllers[nextIndex];
            controller.SetVolume(volumeScale * volume);
            controller.Stop();
            controller.Play();

            // Advance index
            nextIndex++;
            if(nextIndex >= effectControllers.Length)
                nextIndex = 0;
        }

        public void Stop()
        {
            for(int i=0; i<effectControllers.Length; i++)
                effectControllers[i].Stop();
        }
    }
}