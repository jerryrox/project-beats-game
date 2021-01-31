using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard
{
    public class GameModuleProvider
    {
        private IDependencyContainer dependency;


        public GameModuleProvider(IDependencyContainer dependency)
        {
            this.dependency = dependency;
        }

        /// <summary>
        /// Returns the game inputter instance suitable for current game state.
        /// </summary>
        public IGameInputter GetGameInputter(GameParameter parameter)
        {
            var playArea = dependency.Get<PlayAreaContainer>();
            var hitObjectHolder = dependency.Get<HitObjectHolder>();

            if (parameter.IsReplay)
            {
                var replayInputter = new ReplayInputter(parameter.ReplayFile, playArea.HitBar, hitObjectHolder);
                dependency.Inject(replayInputter);
                return replayInputter;
            }
            else
            {
                var localInputter = new LocalPlayerInputter(playArea.HitBar, hitObjectHolder);
                dependency.Inject(localInputter);
                return localInputter;
            }
        }
    }
}