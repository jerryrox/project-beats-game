using PBFramework.Audio;

namespace PBGame.Audio
{
    public interface ISoundTable {

        /// <summary>
        /// Returns the audio of specified lookupName.
        /// </summary>
        IEffectAudio GetAudio(string lookupName);
    }
}