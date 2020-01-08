using PBGame.Skins;
using PBFramework.Audio;

namespace PBGame.Audio
{
    public interface ISoundPooler {

        /// <summary>
        /// Prepares the sound pools for specified skin.
        /// </summary>
        void Prepare(ISkin skin);

        /// <summary>
        /// Plays the sound of specified lookup name.
        /// </summary>
        void Play(string lookupName, float volumeScale = 1f);

        /// <summary>
        /// Stops the sound of specified lookup name.
        /// </summary>
        void Stop(string lookupName);

        /// <summary>
        /// Sets the volume of the pools.
        /// </summary>
        void SetVolume(float volume);
    }
}