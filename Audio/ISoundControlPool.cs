using PBFramework.Audio;

namespace PBGame.Audio
{
    public interface ISoundControlPool {

        /// <summary>
        /// Mounts the specified audio on the controllers.
        /// </summary>
        void SetAudio(IEffectAudio audio);

        /// <summary>
        /// Sets the volume on the controllers.
        /// </summary>
        void SetVolume(float volume);

        /// <summary>
        /// Plays the sound from the next controller.
        /// </summary>
        void Play(float volumeScale = 1f);

        /// <summary>
        /// Stops sound playback on all controllers.
        /// </summary>
        void Stop();
    }
}