using PBGame.Rulesets;
using PBGame.Configurations;
using PBFramework.Data.Bindables;
using PBFramework.Dependencies;

namespace PBGame.UI.Models
{
    public class ModeMenuModel : BaseModel
    {
        /// <summary>
        /// The current selected game mode.
        /// </summary>
        public IReadOnlyBindable<GameModeType> GameMode => GameConfiguration.RulesetMode;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }


        /// <summary>
        /// Sets the current game mode to the specified one.
        /// </summary>
        public void SelectMode(IModeService modeService)
        {
            GameConfiguration.RulesetMode.Value = modeService.GameMode;
        }
    }
}