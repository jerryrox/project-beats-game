using System.Linq;
using System.Collections.Generic;
using PBFramework.Dependencies;

namespace PBGame.Rulesets
{
    public class ModeManager : IModeManager {

        private Dictionary<GameModeType, IModeService> services;


        public ModeManager()
        {
            // TODO: Register more game mode services
            services = new Dictionary<GameModeType, IModeService>()
            {
                { GameModeType.OsuStandard, new Osu.Standard.ModeService() },
                { GameModeType.BeatsStandard, new Beats.Standard.ModeService() },
            };
        }

        public IModeService GetService(GameModeType mode)
        {
            services.TryGetValue(mode, out IModeService value);
            return value;
        }

        public IEnumerable<IModeService> AllServices() => services.Values;

        public IEnumerable<IModeService> PlayableServices() => services.Values.Where(s => s.IsPlayable);
    }
}