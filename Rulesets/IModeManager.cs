using System.Collections.Generic;

namespace PBGame.Rulesets
{
    public interface IModeManager {


        /// <summary>
        /// Returns the game mode service instance for specified mode.
        /// </summary>
        IModeService GetService(GameModeType mode);

        /// <summary>
        /// Returns all available mode servicer instances.
        /// </summary>
        IEnumerable<IModeService> AllServices();

        /// <summary>
        /// Returns all available mode services ready for play.
        /// </summary>
        IEnumerable<IModeService> PlayableServices();
    }
}