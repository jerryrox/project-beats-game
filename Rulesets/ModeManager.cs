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
                // TODO: Hurry and implement osu standard mode service
                { GameModes.OsuStandard, null },
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