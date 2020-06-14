using PBFramework.Audio;

namespace PBGame.Audio
{
    public interface ISoundTable {

        /// <summary>
        /// Returns the audio of specified lookupName.
        /// </summary>
        IEffectAudio GetAudio(string lookupName);

        /// <summary>
        /// Returns whether the sound of specified lookup name exists.
        /// </summary>
        bool Contains(string lookupName);
    }
}