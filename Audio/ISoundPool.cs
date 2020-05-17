using System.Collections.Generic;
using PBFramework.Audio;

namespace PBGame.Audio
{
    public interface ISoundPool {

        /// <summary>
        /// Plays the audio of specified lookup name.
        /// </summary>
        IEffectController Play(string lookupName);

        /// <summary>
        /// Plays the specified effect audio.
        /// </summary>
        IEffectController Play(IEffectAudio effect);

        /// <summary>
        /// Plays the specified range of effects.
        /// </summary>
        IEnumerable<IEffectController> Play(IEnumerable<IEffectAudio> effects);

        /// <summary>
        /// Plays the specified audio in a separate pool for a persistent playback.
        /// </summary>
        IEffectController PlayPersistent(IEffectAudio effect);

        /// <summary>
        /// Sets the volume of the effect controllers.
        /// </summary>
        void SetVolume(float volume);
    }
}