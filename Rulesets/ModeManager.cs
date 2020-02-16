using System.Collections.Generic;
using PBFramework.Dependencies;

namespace PBGame.Rulesets
{
    public class ModeManager : IModeManager {

        private Dictionary<GameModes, IModeService> services;


        public ModeManager(IDependencyContainer dependencies)
        {
            // TODO: Register more game mode services
            services = new Dictionary<GameModes, IModeService>()
            {
                { GameModes.OsuStandard, new Osu.Standard.ModeService(dependencies) },
                { GameModes.BeatsStandard, new Beats.Standard.ModeService(dependencies) },
            };
        }

        public IModeService GetService(GameModes mode)
        {
            services.TryGetValue(mode, out IModeService value);
            return value;
        }

        public IEnumerable<IModeService> AllServices() => services.Values;
    }
}