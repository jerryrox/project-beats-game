using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Rulesets
{
    public class ModeManager : IModeManager {

        private Dictionary<GameModes, IModeService> services = new Dictionary<GameModes, IModeService>()
        {
            { GameModes.OsuStandard, null },
            { GameModes.BeatsStandard, null },
        };


        public IModeService GetService(GameModes mode)
        {
            services.TryGetValue(mode, out IModeService value);
            return value;
        }

        public IEnumerable<IModeService> AllServices() => services.Values;
    }
}